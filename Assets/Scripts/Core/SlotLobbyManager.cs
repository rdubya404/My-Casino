using System;
using System.Collections.Generic;
using MyCasino.Slots.Data;
using UnityEngine;

namespace MyCasino.Slots.Core
{
    public class SlotLobbyManager : MonoBehaviour
    {
        [SerializeField] private VideoSlotMachine machine;
        [SerializeField] private List<VideoSlotDefinition> videoSlots = new();
        [SerializeField] private int defaultSlotIndex;

        private int _selectedIndex;
        public event Action<int> OnSelectedSlotChanged;

        public IReadOnlyList<VideoSlotDefinition> VideoSlots => videoSlots;
        public int SelectedIndex => _selectedIndex;

        private void Start()
        {
            if (videoSlots.Count < 8)
            {
                Debug.LogWarning("Meta Quest build expects 8 unique video slots configured.");
            }

            _selectedIndex = Mathf.Clamp(defaultSlotIndex, 0, Mathf.Max(0, videoSlots.Count - 1));
            LoadSelectedSlot(machine != null ? machine.Credits : 1000);
            OnSelectedSlotChanged?.Invoke(_selectedIndex);
        }

        public void SelectSlot(int index)
        {
            if (index < 0 || index >= videoSlots.Count || machine == null)
            {
                return;
            }

            _selectedIndex = index;
            LoadSelectedSlot(machine.Credits);
            OnSelectedSlotChanged?.Invoke(_selectedIndex);
        }

        public void SelectNextSlot()
        {
            if (videoSlots.Count == 0)
            {
                return;
            }

            var next = (_selectedIndex + 1) % videoSlots.Count;
            SelectSlot(next);
        }

        public void SelectPreviousSlot()
        {
            if (videoSlots.Count == 0)
            {
                return;
            }

            var previous = (_selectedIndex - 1 + videoSlots.Count) % videoSlots.Count;
            SelectSlot(previous);
        }

        private void LoadSelectedSlot(int carryCredits)
        {
            if (machine == null || videoSlots.Count == 0)
            {
                return;
            }

            machine.Configure(videoSlots[_selectedIndex], carryCredits);
        }
    }
}
