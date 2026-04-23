using System;

namespace MyCasino.Slots.Core
{
    [Serializable]
    public readonly struct SlotSpinResult
    {
        public readonly string SlotId;
        public readonly int Bet;
        public readonly int WinAmount;
        public readonly string[,] SymbolGrid;

        public SlotSpinResult(string slotId, int bet, int winAmount, string[,] symbolGrid)
        {
            SlotId = slotId;
            Bet = bet;
            WinAmount = winAmount;
            SymbolGrid = symbolGrid;
        }
    }
}
