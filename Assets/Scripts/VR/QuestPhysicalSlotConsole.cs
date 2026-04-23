using MyCasino.Slots.Core;
using MyCasino.Slots.Data;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace MyCasino.Slots.VR
{
    /// <summary>
    /// Wires physical, in-world XR interactables to slot machine actions.
    /// Assign lever/button colliders with XRSimpleInteractable components.
    /// </summary>
    public class QuestPhysicalSlotConsole : MonoBehaviour
    {
        [SerializeField] private VideoSlotMachine machine;
        [SerializeField] private SlotLobbyManager lobby;
        [SerializeField] private XRSimpleInteractable spinControl;
        [SerializeField] private XRSimpleInteractable nextSlotControl;
        [SerializeField] private XRSimpleInteractable previousSlotControl;
        [SerializeField] private TextMeshPro selectedSlotText;
        [SerializeField] private TextMeshPro creditsText;
        [SerializeField] private TextMeshPro resultText;
        [Header("Cabinet Animation")]
        [SerializeField] private Animator cabinetAnimator;
        [SerializeField] private string spinTriggerName = "SpinPulled";
        [SerializeField] private string nextTriggerName = "NextPressed";
        [SerializeField] private string previousTriggerName = "PrevPressed";
        [SerializeField] private string winTriggerName = "WinPulse";
        [Header("Theme Audio Layers")]
        [SerializeField] private AudioSource controlAudioSource;
        [SerializeField] private AudioSource ambientAudioSource;
        [Header("Theme Lighting")]
        [SerializeField] private Light[] cabinetLights;
        [Header("Button/Lever Motion")]
        [SerializeField] private Transform spinControlVisual;
        [SerializeField] private Transform nextControlVisual;
        [SerializeField] private Transform previousControlVisual;
        [SerializeField, Min(0.002f)] private float controlPressDepth = 0.02f;
        [SerializeField, Min(0.01f)] private float controlPressDuration = 0.08f;
        [Header("Haptics")]
        [SerializeField, Range(0f, 1f)] private float controlHapticAmplitude = 0.45f;
        [SerializeField, Min(0f)] private float controlHapticDuration = 0.08f;
        [SerializeField, Range(0f, 1f)] private float winHapticAmplitude = 0.7f;
        [SerializeField, Min(0f)] private float winHapticDuration = 0.18f;
        private Coroutine _lightPulseRoutine;

        private void OnEnable()
        {
            if (spinControl != null)
            {
                spinControl.selectEntered.AddListener(HandleSpinControlSelected);
            }

            if (nextSlotControl != null)
            {
                nextSlotControl.selectEntered.AddListener(HandleNextControlSelected);
            }

            if (previousSlotControl != null)
            {
                previousSlotControl.selectEntered.AddListener(HandlePreviousControlSelected);
            }

            if (lobby != null)
            {
                lobby.OnSelectedSlotChanged += HandleSlotSelectionChanged;
            }

            if (machine != null)
            {
                machine.OnSpinFinished += HandleSpinFinished;
            }

            RefreshHud();
            ApplyCurrentSlotPresentation();
        }

        private void OnDisable()
        {
            if (spinControl != null)
            {
                spinControl.selectEntered.RemoveAllListeners();
            }

            if (nextSlotControl != null)
            {
                nextSlotControl.selectEntered.RemoveAllListeners();
            }

            if (previousSlotControl != null)
            {
                previousSlotControl.selectEntered.RemoveAllListeners();
            }

            if (lobby != null)
            {
                lobby.OnSelectedSlotChanged -= HandleSlotSelectionChanged;
            }

            if (machine != null)
            {
                machine.OnSpinFinished -= HandleSpinFinished;
            }

            if (_lightPulseRoutine != null)
            {
                StopCoroutine(_lightPulseRoutine);
                _lightPulseRoutine = null;
            }
        }

        public void TrySpin()
        {
            if (machine == null)
            {
                return;
            }

            if (!machine.TrySpin())
            {
                if (resultText != null)
                {
                    resultText.text = "Spin unavailable";
                }
            }

            RefreshHud();
        }

        public void SelectNextSlot()
        {
            if (lobby == null)
            {
                return;
            }

            lobby.SelectNextSlot();
            RefreshHud();
        }

        public void SelectPreviousSlot()
        {
            if (lobby == null)
            {
                return;
            }

            lobby.SelectPreviousSlot();
            RefreshHud();
        }

        private void HandleSlotSelectionChanged(int _)
        {
            ApplyCurrentSlotPresentation();
            RefreshHud();
        }

        private void HandleSpinControlSelected(SelectEnterEventArgs args)
        {
            TriggerCabinetAnimation(spinTriggerName);
            AnimateControlPress(spinControlVisual);
            SendHaptics(args, controlHapticAmplitude, controlHapticDuration);
            PlayControlClip(GetCurrentSlotDefinition()?.LeverPullSfx);
            TrySpin();
        }

        private void HandleNextControlSelected(SelectEnterEventArgs args)
        {
            TriggerCabinetAnimation(nextTriggerName);
            AnimateControlPress(nextControlVisual);
            SendHaptics(args, controlHapticAmplitude, controlHapticDuration);
            PlayControlClip(GetCurrentSlotDefinition()?.ButtonPressSfx);
            SelectNextSlot();
        }

        private void HandlePreviousControlSelected(SelectEnterEventArgs args)
        {
            TriggerCabinetAnimation(previousTriggerName);
            AnimateControlPress(previousControlVisual);
            SendHaptics(args, controlHapticAmplitude, controlHapticDuration);
            PlayControlClip(GetCurrentSlotDefinition()?.ButtonPressSfx);
            SelectPreviousSlot();
        }

        private void HandleSpinFinished(SlotSpinResult result)
        {
            if (resultText != null)
            {
                resultText.text = result.TotalWinAmount > 0
                    ? $"WIN +{result.TotalWinAmount}"
                    : "No win";
            }

            if (result.TotalWinAmount > 0)
            {
                TriggerCabinetAnimation(winTriggerName);
                SendHapticsToSelectingInteractors(spinControl, winHapticAmplitude, winHapticDuration);
                PlayControlClip(GetCurrentSlotDefinition()?.WinStingerSfx);
                PulseThemeLights(result);
            }

            RefreshHud();
        }

        private void TriggerCabinetAnimation(string triggerName)
        {
            if (cabinetAnimator == null || string.IsNullOrWhiteSpace(triggerName))
            {
                return;
            }

            cabinetAnimator.SetTrigger(triggerName);
        }

        private void ApplyCurrentSlotPresentation()
        {
            var definition = GetCurrentSlotDefinition();
            if (definition == null)
            {
                return;
            }

            if (cabinetAnimator != null && definition.ThemeAnimatorOverride != null)
            {
                cabinetAnimator.runtimeAnimatorController = definition.ThemeAnimatorOverride;
            }

            if (ambientAudioSource == null)
            {
                ApplyThemeLights(definition);
                return;
            }

            if (definition.AmbientLoopSfx == null)
            {
                ambientAudioSource.Stop();
                ambientAudioSource.clip = null;
                return;
            }

            if (ambientAudioSource.clip != definition.AmbientLoopSfx)
            {
                ambientAudioSource.clip = definition.AmbientLoopSfx;
                ambientAudioSource.loop = true;
                ambientAudioSource.Play();
            }

            ApplyThemeLights(definition);
        }

        private void PlayControlClip(AudioClip clip)
        {
            if (clip == null || controlAudioSource == null)
            {
                return;
            }

            controlAudioSource.PlayOneShot(clip);
        }

        private void ApplyThemeLights(VideoSlotDefinition definition)
        {
            if (cabinetLights == null || cabinetLights.Length == 0 || definition == null)
            {
                return;
            }

            foreach (var lightRef in cabinetLights)
            {
                if (lightRef == null)
                {
                    continue;
                }

                lightRef.color = definition.ThemeLightColor;
                lightRef.intensity = definition.ThemeBaseLightIntensity;
            }
        }

        private void PulseThemeLights(SlotSpinResult result)
        {
            var definition = GetCurrentSlotDefinition();
            if (definition == null || cabinetLights == null || cabinetLights.Length == 0)
            {
                return;
            }

            if (_lightPulseRoutine != null)
            {
                StopCoroutine(_lightPulseRoutine);
            }

            _lightPulseRoutine = StartCoroutine(PulseThemeLightsRoutine(definition, result));
        }

        private IEnumerator PulseThemeLightsRoutine(VideoSlotDefinition definition, SlotSpinResult result)
        {
            var payoutFactor = machine != null && machine.CurrentBet > 0
                ? Mathf.Clamp(result.TotalWinAmount / (float)machine.CurrentBet, 1f, 10f)
                : 1f;

            var peakIntensity = Mathf.Lerp(
                definition.ThemeBaseLightIntensity,
                definition.ThemeWinLightIntensity,
                Mathf.Clamp01(payoutFactor / 10f));

            var elapsed = 0f;
            while (elapsed < definition.ThemeLightPulseDuration)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / definition.ThemeLightPulseDuration);
                SetLightIntensity(Mathf.Lerp(definition.ThemeBaseLightIntensity, peakIntensity, t), definition.ThemeLightColor);
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < definition.ThemeLightPulseDuration)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / definition.ThemeLightPulseDuration);
                SetLightIntensity(Mathf.Lerp(peakIntensity, definition.ThemeBaseLightIntensity, t), definition.ThemeLightColor);
                yield return null;
            }

            SetLightIntensity(definition.ThemeBaseLightIntensity, definition.ThemeLightColor);
            _lightPulseRoutine = null;
        }

        private void SetLightIntensity(float intensity, Color color)
        {
            foreach (var lightRef in cabinetLights)
            {
                if (lightRef == null)
                {
                    continue;
                }

                lightRef.color = color;
                lightRef.intensity = intensity;
            }
        }

        private void AnimateControlPress(Transform visual)
        {
            if (visual == null)
            {
                return;
            }

            StartCoroutine(PressControlRoutine(visual));
        }

        private IEnumerator PressControlRoutine(Transform visual)
        {
            var originalLocalPosition = visual.localPosition;
            var pressedLocalPosition = originalLocalPosition + Vector3.back * controlPressDepth;
            var elapsed = 0f;

            while (elapsed < controlPressDuration)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / controlPressDuration);
                visual.localPosition = Vector3.Lerp(originalLocalPosition, pressedLocalPosition, t);
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < controlPressDuration)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / controlPressDuration);
                visual.localPosition = Vector3.Lerp(pressedLocalPosition, originalLocalPosition, t);
                yield return null;
            }

            visual.localPosition = originalLocalPosition;
        }

        private static void SendHaptics(SelectEnterEventArgs args, float amplitude, float duration)
        {
            if (args?.interactorObject is not XRBaseControllerInteractor controllerInteractor || controllerInteractor.xrController == null)
            {
                return;
            }

            controllerInteractor.xrController.SendHapticImpulse(amplitude, duration);
        }

        private static void SendHapticsToSelectingInteractors(XRSimpleInteractable interactable, float amplitude, float duration)
        {
            if (interactable == null)
            {
                return;
            }

            foreach (var interactor in interactable.interactorsSelecting)
            {
                if (interactor is XRBaseControllerInteractor controllerInteractor && controllerInteractor.xrController != null)
                {
                    controllerInteractor.xrController.SendHapticImpulse(amplitude, duration);
                }
            }
        }

        private void RefreshHud()
        {
            if (selectedSlotText != null && lobby != null && lobby.VideoSlots.Count > 0)
            {
                selectedSlotText.text = $"Slot: {lobby.VideoSlots[lobby.SelectedIndex].DisplayName}";
            }

            if (creditsText != null && machine != null)
            {
                creditsText.text = $"Credits: {machine.Credits} | Free: {machine.FreeSpinsRemaining} | Jackpot: {machine.ProgressiveJackpotPool}";
            }
        }

        private VideoSlotDefinition GetCurrentSlotDefinition()
        {
            if (lobby == null || lobby.VideoSlots.Count == 0 || lobby.SelectedIndex < 0 || lobby.SelectedIndex >= lobby.VideoSlots.Count)
            {
                return null;
            }

            return lobby.VideoSlots[lobby.SelectedIndex];
        }
    }
}
