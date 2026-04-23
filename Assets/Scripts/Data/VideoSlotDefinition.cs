using System.Collections.Generic;
using UnityEngine;

namespace MyCasino.Slots.Data
{
    [System.Serializable]
    public class PaylinePattern
    {
        [field: SerializeField] public string Id { get; private set; } = "line";
        [field: SerializeField] public int[] ReelRows { get; private set; } = { 1, 1, 1, 1, 1 };
    }

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
        [field: SerializeField] public List<PaylinePattern> Paylines { get; private set; } = new();
        [field: Header("Free Spins")]
        [field: SerializeField] public string FreeSpinTriggerSymbolId { get; private set; } = "scatter";
        [field: SerializeField, Min(1)] public int FreeSpinTriggerCount { get; private set; } = 3;
        [field: SerializeField, Min(1)] public int FreeSpinAwardCount { get; private set; } = 5;
        [field: Header("Bonus Mini-Game")]
        [field: SerializeField] public string BonusTriggerSymbolId { get; private set; } = "bonus";
        [field: SerializeField, Min(1)] public int BonusTriggerCount { get; private set; } = 3;
        [field: SerializeField, Min(1)] public int BonusPayoutMultiplierMin { get; private set; } = 2;
        [field: SerializeField, Min(1)] public int BonusPayoutMultiplierMax { get; private set; } = 8;

        public bool IsValid => Symbols != null && Symbols.Count > 0 && ReelCount > 0 && RowCount > 0;
    }
}
