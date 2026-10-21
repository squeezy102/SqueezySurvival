# Equipment

**Status:** Draft

What the survivor wears and holds.

## Slots

| Slot | Holds |
|---|---|
| Tool hand | The main tool or weapon |
| Offhand | A secondary item (torch, shield, a second hand basket), or a quiver or ammo pouch for a bow or sling. Unavailable while holding a two-handed weapon. |
| Head | Hats, helmets |
| Neck | Necklaces, scarves |
| Chest | Shirts, armor |
| Belt | Belts (a [carrier](Inventory.md#carriers)) |
| Legs | Trousers, leg wraps |
| Shoes | Footwear (protects against hazards such as urchins) |
| Gloves | Hand protection |
| Trinket 1 | Charms, small items |
| Trinket 2 | Charms, small items |
| Back | One of: backpack, cloak, cape or jacket |

### Handedness

The player chooses the survivor's dominant hand, left or right, at character select (resolves OQ-040). The choice is required and has no default. The dominant hand is the tool hand; the other is the offhand.

Handedness has **no gameplay effect** beyond deciding which side holds the tool. It exists so left-handed players are recognized. [Arm injuries](Injuries.md#body-locations) affect whichever hand is on the injured side. Two-handed weapons and the offhand: [Combat: weapons](Combat.md#weapons).

## Loadouts

Gearing for utility and gearing for combat are a real trade-off.

- **Tools work as weapons, but worse.** A flint knife is a Melee weapon and can be paired with a shield, but deals less damage than a short sword or another purpose-made weapon.
- **Combat gear costs utility.** A shield in the offhand displaces a hand basket or torch, so the survivor gathers and carries less.
- **Gear is locked during combat** ([Combat](Combat.md#weapons)), so the player is rewarded for wearing a combat loadout in dangerous places and pays for being caught in gathering gear.

### Saved loadouts

Saved loadouts come from placeable **wardrobe buildings** (resolves OQ-043). Each stores whole gear sets; swapping exchanges what the survivor is wearing with a stored set in one action, in the zone where the building stands.

| Building | Sets stored | Availability |
|---|---|---|
| Stick dummy | 1: whatever is put on the dummy | Early |
| Wardrobe | More | Later woodworking blueprints |
| Armoire | More still | Later woodworking blueprints |

Gear can also be carried in the [inventory](Inventory.md) and equipped piece by piece; its weight counts against carrying capacity like anything else. A wardrobe building only adds the one-action swap at home.

Swap duration and exact set counts are set with content.

## Treatments and equipment

Injury treatments attach to the injury, not to an equipment slot ([Injuries and Health](Injuries.md#treatment)). A treatment can block equipment at its location, for example:

### Hands and arms

| Condition | Tool-hand side | Offhand side |
|---|---|---|
| Bandaged | No gloves; slight work penalty on tool actions | No gloves; reduced carrying (a carried basket holds less, or nothing can be carried on that side) |
| Broken bone (splinted) | Slot unusable until healed: no gloves, nothing held | Slot unusable until healed: no gloves, nothing held |
| Any arm injury, in combat | Lowered damage dealt ([Combat](Combat.md)) | - |

Broken bones must be splinted to heal properly.

### Other locations

| Treatment | Effect |
|---|---|
| Head wrap | Blocks the head slot |
| Splinted leg | Slows movement; may block the legs slot |
