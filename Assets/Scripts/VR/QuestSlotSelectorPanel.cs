using MyCasino.Slots.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyCasino.Slots.VR
{
    public class QuestSlotSelectorPanel : MonoBehaviour
    {
        [SerializeField] private SlotLobbyManager lobby;
        [SerializeField] private Button buttonTemplate;
        [SerializeField] private Transform buttonRoot;
        [SerializeField] private TextMeshPro titleText;

        private void Start()
        {
            if (lobby != null)
            {
                lobby.OnSelectedSlotChanged += HandleSlotChanged;
            }

            BuildButtons();
            RefreshTitle();
        }

        private void OnDestroy()
        {
            if (lobby != null)
            {
                lobby.OnSelectedSlotChanged -= HandleSlotChanged;
            }
        }

        private void BuildButtons()
        {
            if (lobby == null || buttonTemplate == null || buttonRoot == null)
            {
                return;
            }

            buttonTemplate.gameObject.SetActive(false);

            for (var i = 0; i < lobby.VideoSlots.Count; i++)
            {
                var index = i;
                var slot = lobby.VideoSlots[i];
                var button = Instantiate(buttonTemplate, buttonRoot);
                button.gameObject.SetActive(true);
                button.GetComponentInChildren<TextMeshProUGUI>().text = slot.DisplayName;
                button.onClick.AddListener(() =>
                {
                    lobby.SelectSlot(index);
                });
            }
        }

        private void HandleSlotChanged(int _)
        {
            RefreshTitle();
        }

        private void RefreshTitle()
        {
            if (lobby == null || titleText == null || lobby.VideoSlots.Count == 0)
            {
                return;
            }

            titleText.text = $"Selected: {lobby.VideoSlots[lobby.SelectedIndex].DisplayName}";
        }
    }
}
