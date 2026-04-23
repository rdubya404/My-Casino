# Meta Quest 3 VR Slot Machine (8 Unique Video Slots)

This project includes a Unity-ready architecture for a VR slot machine experience tailored for **Meta Quest 3**.

## Included gameplay systems

- `VideoSlotMachine`: weighted reel spin logic, payout calculation, and credit balance.
- `SlotLobbyManager`: switches between slot games while carrying credits.
- `QuestSpinInteractor`: XR interaction hook for grabbing/pressing a spin control in VR.
- `QuestSlotSelectorPanel`: runtime button generation for selecting one of the configured slot games.
- `VideoSlotDefinition` and `SlotSymbol`: ScriptableObject-driven content for unique slot themes.

## 8 unique slot game themes to configure

Create 8 `VideoSlotDefinition` assets (one per game):

1. Neon Nights
2. Pharaoh's Relics
3. Deep Sea Deluxe
4. Dragon Fortune
5. Galactic Gold
6. Wild West Rush
7. Jungle Temple
8. Cyber Heist

Each slot should have:
- Unique symbols (at least 8 symbols per slot recommended)
- Distinct spin/win SFX
- A distinct `ThemeColor`
- Appropriate `BetUnit`

## Unity + Quest 3 scene setup

1. Install dependencies:
   - Unity 2022 LTS or later
   - XR Plugin Management
   - OpenXR
   - XR Interaction Toolkit
   - TextMeshPro
2. Enable Android + OpenXR + Meta Quest feature group in Project Settings.
3. Add an XR Origin to the scene.
4. Add a slot machine cabinet GameObject with:
   - `VideoSlotMachine`
   - `AudioSource`
5. Add a lobby controller GameObject with:
   - `SlotLobbyManager`
   - assign the 8 `VideoSlotDefinition` assets
   - reference the machine
6. Add a spin button object with:
   - collider
   - `XRSimpleInteractable`
   - `QuestSpinInteractor`
7. Add a physical in-world slot console (lever/buttons) with:
   - colliders + `XRSimpleInteractable` on each physical control
   - `QuestPhysicalSlotConsole` on the console root
   - assign spin / next slot / previous slot controls
   - assign optional in-world text labels for selected slot, credits, and result
8. (Optional legacy UI) Add a world-space canvas panel with:
   - `QuestSlotSelectorPanel`
   - button template + container root
9. (Optional hand tracking) Add an input controller GameObject with:
   - `QuestHandGestureController`
   - references to `VideoSlotMachine` and `SlotLobbyManager`
10. Build Settings:
   - Platform: Android
   - Texture Compression: ASTC
   - Target Device: Quest 3

## Payout logic currently implemented

- Reels spin left-to-right with per-reel delay.
- Paylines support:
  - Horizontal rows
  - Diagonal lines
  - Zig-zag lines
- 3+ matching symbols from the leftmost reel pay:
  - `bet * payoutMultiplier * (streak - 2)`
- `VideoSlotDefinition.Paylines` can override defaults with custom line paths (one row index per reel).

## Free spins and bonus mini-game

- Free spins:
  - Triggered by landing `FreeSpinTriggerCount` or more `FreeSpinTriggerSymbolId` symbols anywhere in the grid.
  - Awards `FreeSpinAwardCount` spins.
  - Free spins do not consume credits.
- Bonus mini-game:
  - Triggered by landing `BonusTriggerCount` or more `BonusTriggerSymbolId` symbols anywhere in the grid.
  - Runs a simple random bonus-pick simulation that awards `bet * randomMultiplier`.
  - Multiplier range is controlled by `BonusPayoutMultiplierMin` and `BonusPayoutMultiplierMax`.

## Progressive jackpot tracking

- Every paid spin contributes `bet * ProgressiveContributionPercent` into a shared jackpot pool.
- Current jackpot value is exposed at runtime via `VideoSlotMachine.ProgressiveJackpotPool`.
- Jackpot is triggered by landing `JackpotTriggerCount` or more `JackpotTriggerSymbolId` symbols anywhere in the grid.
- On jackpot hit:
  - Player receives the entire current pool value.
  - Pool resets to `ProgressiveJackpotSeed`.

## Hand-tracking gestures (Quest 3)

`QuestHandGestureController` supports gesture-first interaction without button presses:

- **Right-hand pinch (thumb + index):** spin the active slot.
- **Left-hand pinch + swipe right:** select next slot theme.
- **Left-hand pinch + swipe left:** select previous slot theme.

Gesture thresholds are configurable in the component:
- `Pinch Distance Threshold`
- `Swipe Distance Threshold`
- `Spin Cooldown Seconds`

## Next recommended upgrades

- Add cabinet animations and haptics tied to lever pulls and button presses.
