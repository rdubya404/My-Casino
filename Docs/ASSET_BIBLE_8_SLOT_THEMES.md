# Asset Bible: 8 Video Slot Themes (Meta Quest 3 VR)

This document defines the art and content direction for the eight slot themes used by `VideoSlotDefinition` assets.

## Global art direction

- **Style target:** stylized realism (readable at VR distance, premium casino finish, not photoreal).
- **Readability rule:** silhouette-first symbols with high contrast outer shapes and emissive accents.
- **Material language:** metallic trim + lacquered enamel + subtle emissive edge glow.
- **Performance guardrails (Quest 3):**
  - symbol icon textures: 512x512 or 1024x1024 ASTC
  - avoid tiny high-frequency details that shimmer in headset
  - keep VFX alpha overdraw moderate

## Global rarity tiers

Use this baseline rarity model across all themes and tune by game feel:

- **Common**: 38-48% combined weight
- **Uncommon**: 26-34% combined weight
- **Rare**: 14-20% combined weight
- **Epic**: 4-8% combined weight
- **Legendary**: 1-3% combined weight
- **Special triggers** (`scatter`, `bonus`, `jackpot`): 0.3-1.5% each

## Theme 1 — Neon Nights

- **Art direction:** futuristic rooftop casino, rain-soaked neon signage, synthwave energy.
- **Palette:** `#00F0FF`, `#FF2AD4`, `#7C4DFF`, `#111827`, `#F9FAFB`.
- **Symbol set + rarity:**
  - Common: neon chip, lucky 7 sign, diamond tube
  - Uncommon: holo cocktail, electric tiger, city crown
  - Rare: pulse phoenix
  - Epic: cyber roulette eye
  - Legendary: neon dragon crest
  - Special: scatter = synth star, bonus = glitch cube, jackpot = skyline sigil
- **VFX language:** scanlines, chromatic trails, bloom spikes, horizontal glitch tears.
- **UI motifs:** glassmorphism cards, magenta/cyan gradients, angular techno dividers.

## Theme 2 — Pharaoh's Relics

- **Art direction:** moonlit tomb vault with gold filigree and magical hieroglyph light.
- **Palette:** `#D4AF37`, `#1E3A8A`, `#0F766E`, `#5B3A29`, `#F8E7B9`.
- **Symbol set + rarity:**
  - Common: scarab coin, ankh charm, papyrus scroll
  - Uncommon: cobra crown, lotus idol, canopic urn
  - Rare: sun disk relic
  - Epic: priestess mask
  - Legendary: awakened sphinx eye
  - Special: scatter = winged scarab, bonus = sealed sarcophagus, jackpot = pharaoh cartouche
- **VFX language:** sand wisps, rune reveals, dust motes, gold spark showers.
- **UI motifs:** carved stone panels, gilded borders, hieroglyph dividers.

## Theme 3 — Deep Sea Deluxe

- **Art direction:** luxury submarine lounge near bioluminescent reef ruins.
- **Palette:** `#00A6FB`, `#006494`, `#051923`, `#7BDFF2`, `#CDECF6`.
- **Symbol set + rarity:**
  - Common: pearl token, coral crest, anchor charm
  - Uncommon: manta sigil, captain sextant, treasure map
  - Rare: leviathan tooth
  - Epic: abyssal crown
  - Legendary: kraken heart gem
  - Special: scatter = jelly bloom, bonus = treasure chest, jackpot = trident emblem
- **VFX language:** caustic light ripples, bubble plumes, plankton glows, pressure pulses.
- **UI motifs:** curved porthole frames, sonar rings, aqua holographic labels.

## Theme 4 — Dragon Fortune

- **Art direction:** celestial temple market with silk banners and lucky fire motifs.
- **Palette:** `#C1121F`, `#F4A261`, `#F6BD60`, `#2A9D8F`, `#1D3557`.
- **Symbol set + rarity:**
  - Common: coin knot, jade ring, lantern token
  - Uncommon: tiger seal, lucky fan, cloud mirror
  - Rare: qilin crest
  - Epic: jade emperor tablet
  - Legendary: golden dragon orb
  - Special: scatter = red envelope, bonus = fortune drum, jackpot = imperial dragon seal
- **VFX language:** ember curls, ribbon trails, ink-cloud reveals, firecracker pops.
- **UI motifs:** lacquer red panels, gold trim, calligraphic frame corners.

## Theme 5 — Galactic Gold

- **Art direction:** interstellar resort vault orbiting a bright binary star.
- **Palette:** `#F4D35E`, `#0B132B`, `#1C2541`, `#5BC0BE`, `#E0FBFC`.
- **Symbol set + rarity:**
  - Common: star chip, ion ingot, orbit ring
  - Uncommon: comet shard, astronaut crest, moon vault key
  - Rare: nebula prism
  - Epic: singularity core
  - Legendary: astral monarch helm
  - Special: scatter = stardust burst, bonus = cargo pod, jackpot = galaxy crown
- **VFX language:** star streaks, lens flares, warp swirls, parallax dust fields.
- **UI motifs:** holographic star-map grids, arc dials, chrome sci-fi frames.

## Theme 6 — Wild West Rush

- **Art direction:** high-end frontier casino train at sunset canyon pass.
- **Palette:** `#BC6C25`, `#DDA15E`, `#FEFAE0`, `#283618`, `#6C584C`.
- **Symbol set + rarity:**
  - Common: sheriff chip, horseshoe, whiskey token
  - Uncommon: revolver emblem, saddle icon, poker spade buckle
  - Rare: bounty badge
  - Epic: outlaw ace
  - Legendary: golden longhorn crest
  - Special: scatter = tumbleweed star, bonus = lockbox safe, jackpot = railroad crown
- **VFX language:** dust kicks, muzzle-flash sparks, spur glints, lasso arcs.
- **UI motifs:** engraved brass plates, stitched leather tabs, wood-inlay separators.

## Theme 7 — Jungle Temple

- **Art direction:** overgrown sky temple with rain, ruins, and luminous flora.
- **Palette:** `#2D6A4F`, `#40916C`, `#95D5B2`, `#1B4332`, `#D8F3DC`.
- **Symbol set + rarity:**
  - Common: vine coin, frog totem, stone leaf
  - Uncommon: jaguar mask, orchid charm, obsidian blade
  - Rare: temple sun idol
  - Epic: serpent altar gem
  - Legendary: rainforest guardian crown
  - Special: scatter = firefly cluster, bonus = relic altar chest, jackpot = emerald monolith sigil
- **VFX language:** pollen clouds, rain streaks, leaf bursts, bioluminescent spores.
- **UI motifs:** moss-carved stone, vine overlays, glyph circles.

## Theme 8 — Cyber Heist

- **Art direction:** elite digital vault breach with tactical neon and stealth HUD overlays.
- **Palette:** `#14F195`, `#00BBF9`, `#3A86FF`, `#0B0F1A`, `#E6FFF9`.
- **Symbol set + rarity:**
  - Common: access chip, data shard, lock node
  - Uncommon: drone badge, EMP canister, keycard prism
  - Rare: breach worm insignia
  - Epic: black-ice core
  - Legendary: quantum vault key
  - Special: scatter = packet burst, bonus = secure cache cube, jackpot = root access sigil
- **VFX language:** voxel breakup, circuit arcs, code rain, pulse grids.
- **UI motifs:** tactical HUD panels, terminal typography, segmented progress bars.

## Cross-theme UI standards

- **Top HUD:** credits, bet, free spins, jackpot must always be present.
- **Result callout:** win text color maps by payout tier (low/med/high).
- **Slot select:** theme icon + theme name + one-line fantasy tagline.
- **Accessibility:** never rely on hue alone; pair color with iconography and contrast.

## Audio direction by rarity tier

- **Common wins:** short 200-350ms tick/chime.
- **Uncommon/Rare:** layered transient + tonal tail 500-900ms.
- **Epic/Legendary:** stinger + sub hit + celebratory top sparkle.
- **Bonus/Jackpot:** unique signature motif per theme; reserve distinct frequency space.

## Delivery checklist per theme

- 10+ symbol icons (including scatter/bonus/jackpot)
- 1 ambient loop
- 3 control SFX (lever, button, win stinger)
- 1 animator override profile
- 1 light profile (base/win pulse)
- 3 payout burst variants (low/medium/high)
