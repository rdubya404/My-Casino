using System;

namespace MyCasino.Slots.Core
{
    [Serializable]
    public readonly struct SlotSpinResult
    {
        public readonly string SlotId;
        public readonly int Bet;
        public readonly int WinAmount;
        public readonly int BonusWinAmount;
        public readonly int JackpotWinAmount;
        public readonly int FreeSpinsAwarded;
        public readonly bool BonusTriggered;
        public readonly bool JackpotTriggered;
        public readonly bool UsedFreeSpin;
        public readonly string[,] SymbolGrid;
        public int TotalWinAmount => WinAmount + BonusWinAmount + JackpotWinAmount;

        public SlotSpinResult(
            string slotId,
            int bet,
            int winAmount,
            int bonusWinAmount,
            int jackpotWinAmount,
            int freeSpinsAwarded,
            bool bonusTriggered,
            bool jackpotTriggered,
            bool usedFreeSpin,
            string[,] symbolGrid)
        {
            SlotId = slotId;
            Bet = bet;
            WinAmount = winAmount;
            BonusWinAmount = bonusWinAmount;
            JackpotWinAmount = jackpotWinAmount;
            FreeSpinsAwarded = freeSpinsAwarded;
            BonusTriggered = bonusTriggered;
            JackpotTriggered = jackpotTriggered;
            UsedFreeSpin = usedFreeSpin;
            SymbolGrid = symbolGrid;
        }
    }
}
