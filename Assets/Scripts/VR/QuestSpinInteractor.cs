using MyCasino.Slots.Core;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace MyCasino.Slots.VR
{
    [RequireComponent(typeof(XRSimpleInteractable))]
    public class QuestSpinInteractor : MonoBehaviour
    {
        [SerializeField] private VideoSlotMachine machine;
        [SerializeField] private TextMeshPro creditsText;
        [SerializeField] private TextMeshPro resultText;
        [SerializeField] private AudioSource uiAudioSource;

        private XRSimpleInteractable _interactable;

        private void Awake()
        {
            _interactable = GetComponent<XRSimpleInteractable>();
            _interactable.selectEntered.AddListener(_ => TrySpin());

            if (machine != null)
            {
                machine.OnSpinStarted += HandleSpinStart;
                machine.OnSpinFinished += HandleSpinFinished;
            }

            RefreshCredits();
        }

        private void OnDestroy()
        {
            if (_interactable != null)
            {
                _interactable.selectEntered.RemoveAllListeners();
            }

            if (machine != null)
            {
                machine.OnSpinStarted -= HandleSpinStart;
                machine.OnSpinFinished -= HandleSpinFinished;
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
                if (resultText != null) resultText.text = "Not enough credits";
                return;
            }

            RefreshCredits();
        }

        private void HandleSpinStart(VideoSlotMachine slotMachine)
        {
            if (resultText != null) resultText.text = $"Spinning {slotMachine.Definition.DisplayName}...";
            if (uiAudioSource != null && slotMachine.Definition.SpinSfx != null)
            {
                uiAudioSource.PlayOneShot(slotMachine.Definition.SpinSfx);
            }
        }

        private void HandleSpinFinished(SlotSpinResult result)
        {
            if (resultText != null)
            {
                if (result.TotalWinAmount > 0)
                {
                    resultText.text = $"WIN +{result.TotalWinAmount}";
                }
                else
                {
                    resultText.text = "No win";
                }

                if (result.FreeSpinsAwarded > 0)
                {
                    resultText.text += $" | Free Spins +{result.FreeSpinsAwarded}";
                }

                if (result.BonusTriggered)
                {
                    resultText.text += $" | Bonus +{result.BonusWinAmount}";
                }
            }

            if (uiAudioSource != null && result.TotalWinAmount > 0 && machine.Definition.WinSfx != null)
            {
                uiAudioSource.PlayOneShot(machine.Definition.WinSfx);
            }

            RefreshCredits();
        }

        private void RefreshCredits()
        {
            if (creditsText != null && machine != null)
            {
                creditsText.text = $"Credits: {machine.Credits} | Free Spins: {machine.FreeSpinsRemaining}";
            }
        }
    }
}
