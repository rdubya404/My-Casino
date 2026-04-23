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
7. Add a world-space canvas panel with:
   - `QuestSlotSelectorPanel`
   - button template + container root
8. Build Settings:
   - Platform: Android
   - Texture Compression: ASTC
   - Target Device: Quest 3

## Payout logic currently implemented

- Reels spin left-to-right with per-reel delay.
- Horizontal paylines are evaluated across each row.
- 3+ matching symbols from the leftmost reel pay:
  - `bet * payoutMultiplier * (streak - 2)`

## Next recommended upgrades

- Add paylines beyond horizontal rows (diagonals, zig-zag).
- Add free spins and bonus mini-games.
- Add progressive jackpot tracking.
- Add hand-tracking gestures for spin and slot selection.
- Replace temporary UI buttons with physical in-world VR controls.
