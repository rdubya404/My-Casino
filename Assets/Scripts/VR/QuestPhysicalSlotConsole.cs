using MyCasino.Slots.Core;
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

        private void OnEnable()
        {
            if (spinControl != null)
            {
                spinControl.selectEntered.AddListener(_ => TrySpin());
            }

            if (nextSlotControl != null)
            {
                nextSlotControl.selectEntered.AddListener(_ => SelectNextSlot());
            }

            if (previousSlotControl != null)
            {
                previousSlotControl.selectEntered.AddListener(_ => SelectPreviousSlot());
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
            RefreshHud();
        }

        private void HandleSpinFinished(SlotSpinResult result)
        {
            if (resultText != null)
            {
                resultText.text = result.TotalWinAmount > 0
                    ? $"WIN +{result.TotalWinAmount}"
                    : "No win";
            }

            RefreshHud();
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
    }
}
