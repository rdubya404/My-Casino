using System.Collections.Generic;
using UnityEngine;

namespace MyCasino.Slots.Data
{
    [CreateAssetMenu(menuName = "MyCasino/Slots/Video Slot Definition", fileName = "VideoSlotDefinition")]
    public class VideoSlotDefinition : ScriptableObject
    {
        [field: SerializeField] public string SlotId { get; private set; } = "new-slot";
        [field: SerializeField] public string DisplayName { get; private set; } = "New Slot";
        [field: SerializeField] public Color ThemeColor { get; private set; } = Color.magenta;
        [field: SerializeField] public AudioClip SpinSfx { get; private set; }
        [field: SerializeField] public AudioClip WinSfx { get; private set; }
        [field: SerializeField, Min(1)] public int BetUnit { get; private set; } = 10;
        [field: SerializeField, Min(1)] public int ReelCount { get; private set; } = 5;
        [field: SerializeField, Min(1)] public int RowCount { get; private set; } = 3;
        [field: SerializeField] public List<SlotSymbol> Symbols { get; private set; } = new();

        public bool IsValid => Symbols != null && Symbols.Count > 0 && ReelCount > 0 && RowCount > 0;
    }
}
