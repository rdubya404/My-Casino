using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyCasino.Slots.Data;
using UnityEngine;

namespace MyCasino.Slots.Core
{
    public class VideoSlotMachine : MonoBehaviour
    {
        [SerializeField] private VideoSlotDefinition definition;
        [SerializeField, Min(0)] private int initialCredits = 1000;
        [SerializeField, Min(0.1f)] private float reelStopDelay = 0.2f;

        private readonly List<float> _weightLookup = new();
        private bool _isSpinning;
        private int _freeSpinsRemaining;

        public event Action<VideoSlotMachine> OnSpinStarted;
        public event Action<SlotSpinResult> OnSpinFinished;
        public event Action<VideoSlotMachine> OnBonusMiniGameStarted;
        public event Action<int> OnBonusMiniGameFinished;

        public int Credits { get; private set; }
        public int CurrentBet => definition != null ? definition.BetUnit : 0;
        public VideoSlotDefinition Definition => definition;
        public int FreeSpinsRemaining => _freeSpinsRemaining;

        private void Awake()
        {
            Credits = initialCredits;
            RebuildWeightLookup();
        }

        public void Configure(VideoSlotDefinition slotDefinition, int carryCredits)
        {
            definition = slotDefinition;
            Credits = Mathf.Max(0, carryCredits);
            _freeSpinsRemaining = 0;
            RebuildWeightLookup();
        }

        public bool TrySpin()
        {
            if (_isSpinning || definition == null || !definition.IsValid)
            {
                return false;
            }

            if (_freeSpinsRemaining <= 0 && Credits < CurrentBet)
            {
                return false;
            }

            var usingFreeSpin = _freeSpinsRemaining > 0;
            if (usingFreeSpin)
            {
                _freeSpinsRemaining--;
            }
            else
            {
                Credits -= CurrentBet;
            }

            StartCoroutine(SpinRoutine(usingFreeSpin));
            return true;
        }

        public void AddCredits(int amount)
        {
            Credits = Mathf.Max(0, Credits + amount);
        }

        private IEnumerator SpinRoutine(bool usedFreeSpin)
        {
            _isSpinning = true;
            OnSpinStarted?.Invoke(this);

            var grid = new string[definition.ReelCount, definition.RowCount];

            for (var reel = 0; reel < definition.ReelCount; reel++)
            {
                for (var row = 0; row < definition.RowCount; row++)
                {
                    var symbol = SampleWeightedSymbol();
                    grid[reel, row] = symbol.Id;
                }

                yield return new WaitForSeconds(reelStopDelay);
            }

            var lineWin = CalculateWin(grid);
            var awardedFreeSpins = GetAwardedFreeSpins(grid);
            var bonusTriggered = IsBonusTriggered(grid);
            var bonusWin = 0;

            if (awardedFreeSpins > 0)
            {
                _freeSpinsRemaining += awardedFreeSpins;
            }

            if (bonusTriggered)
            {
                OnBonusMiniGameStarted?.Invoke(this);
                yield return new WaitForSeconds(0.5f);
                bonusWin = PlayBonusMiniGame();
                OnBonusMiniGameFinished?.Invoke(bonusWin);
            }

            Credits += lineWin + bonusWin;

            var result = new SlotSpinResult(
                definition.SlotId,
                CurrentBet,
                lineWin,
                bonusWin,
                awardedFreeSpins,
                bonusTriggered,
                usedFreeSpin,
                grid);
            OnSpinFinished?.Invoke(result);
            _isSpinning = false;
        }

        private void RebuildWeightLookup()
        {
            _weightLookup.Clear();

            if (definition == null || definition.Symbols == null)
            {
                return;
            }

            var cumulative = 0f;
            foreach (var symbol in definition.Symbols)
            {
                cumulative += symbol != null ? symbol.Weight : 0f;
                _weightLookup.Add(cumulative);
            }
        }

        private SlotSymbol SampleWeightedSymbol()
        {
            var target = UnityEngine.Random.value * _weightLookup[^1];
            for (var i = 0; i < _weightLookup.Count; i++)
            {
                if (target <= _weightLookup[i])
                {
                    return definition.Symbols[i];
                }
            }

            return definition.Symbols[^1];
        }

        private int CalculateWin(string[,] grid)
        {
            var win = 0;
            foreach (var payline in GetActivePaylines())
            {
                var firstRow = payline[0];
                var streakSymbol = grid[0, firstRow];
                var streak = 1;

                for (var reel = 1; reel < definition.ReelCount; reel++)
                {
                    var row = payline[reel];
                    if (grid[reel, row] != streakSymbol)
                    {
                        break;
                    }

                    streak++;
                }

                if (streak >= 3)
                {
                    var payoutMultiplier = GetMultiplier(streakSymbol);
                    win += CurrentBet * payoutMultiplier * (streak - 2);
                }
            }

            return win;
        }

        private int GetAwardedFreeSpins(string[,] grid)
        {
            if (string.IsNullOrWhiteSpace(definition.FreeSpinTriggerSymbolId))
            {
                return 0;
            }

            var matches = CountSymbol(grid, definition.FreeSpinTriggerSymbolId);
            return matches >= definition.FreeSpinTriggerCount
                ? definition.FreeSpinAwardCount
                : 0;
        }

        private bool IsBonusTriggered(string[,] grid)
        {
            if (string.IsNullOrWhiteSpace(definition.BonusTriggerSymbolId))
            {
                return false;
            }

            var matches = CountSymbol(grid, definition.BonusTriggerSymbolId);
            return matches >= definition.BonusTriggerCount;
        }

        private int PlayBonusMiniGame()
        {
            var min = Mathf.Min(definition.BonusPayoutMultiplierMin, definition.BonusPayoutMultiplierMax);
            var max = Mathf.Max(definition.BonusPayoutMultiplierMin, definition.BonusPayoutMultiplierMax);
            var selectedMultiplier = UnityEngine.Random.Range(min, max + 1);
            return CurrentBet * selectedMultiplier;
        }

        private int CountSymbol(string[,] grid, string symbolId)
        {
            var matches = 0;
            for (var reel = 0; reel < definition.ReelCount; reel++)
            {
                for (var row = 0; row < definition.RowCount; row++)
                {
                    if (grid[reel, row] == symbolId)
                    {
                        matches++;
                    }
                }
            }

            return matches;
        }

        private IEnumerable<int[]> GetActivePaylines()
        {
            var emittedConfiguredPayline = false;
            if (definition.Paylines != null && definition.Paylines.Count > 0)
            {
                foreach (var configured in definition.Paylines)
                {
                    if (configured?.ReelRows == null || configured.ReelRows.Length != definition.ReelCount)
                    {
                        continue;
                    }

                    if (configured.ReelRows.Any(row => row < 0 || row >= definition.RowCount))
                    {
                        continue;
                    }

                    emittedConfiguredPayline = true;
                    yield return configured.ReelRows;
                }
            }

            if (!emittedConfiguredPayline)
            {
                foreach (var fallback in BuildDefaultPaylines())
                {
                    yield return fallback;
                }
            }
        }

        private IEnumerable<int[]> BuildDefaultPaylines()
        {
            // Horizontal paylines.
            for (var row = 0; row < definition.RowCount; row++)
            {
                var line = new int[definition.ReelCount];
                for (var reel = 0; reel < definition.ReelCount; reel++)
                {
                    line[reel] = row;
                }

                yield return line;
            }

            if (definition.RowCount < 3 || definition.ReelCount < 3)
            {
                yield break;
            }

            // Diagonals.
            yield return MirrorPattern(0, 1, 2, 1, 0);
            yield return MirrorPattern(2, 1, 0, 1, 2);

            // Zig-zags.
            yield return MirrorPattern(1, 0, 1, 2, 1);
            yield return MirrorPattern(1, 2, 1, 0, 1);
        }

        private int[] MirrorPattern(params int[] pattern)
        {
            var line = new int[definition.ReelCount];
            for (var reel = 0; reel < definition.ReelCount; reel++)
            {
                line[reel] = pattern[reel % pattern.Length];
            }

            return line;
        }

        private int GetMultiplier(string symbolId)
        {
            foreach (var symbol in definition.Symbols)
            {
                if (symbol != null && symbol.Id == symbolId)
                {
                    return Mathf.Max(1, symbol.PayoutMultiplier);
                }
            }

            return 1;
        }
    }
}
