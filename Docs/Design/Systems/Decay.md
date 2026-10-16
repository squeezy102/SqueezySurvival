# Decay

**Status:** Draft

Decay is a resource sink: items that are not used or protected deteriorate over time.

## What decays

Every item decays, except permanent buildings such as a forge, which never decay or break on their own. Disasters such as a [wildfire](Weather.md#wildfire) can still damage or destroy them. Organic items decay over days; inorganic items (stone, shells, metal) decay far more slowly than anything else - a rock lasts on the ground a very long time, but not forever (resolves OQ-026).

| Item type | Examples | Result when fully decayed |
|---|---|---|
| Food | Fish, fruit, meat | Converts: rot, slime |
| Plant matter | Leaves, seaweed | Converts: dried leaves, then rot or dirt |
| Wood | Sticks, logs | Removed |
| Inorganic | Rocks, stones, shells | Removed, after a very long time |
| Crafted items | Tools, torches | Breaks; leaves only part of its materials, usually not the important ones |
| Permanent buildings | Forge | Never decays; disasters can damage or destroy them |

Some results are themselves useful resources (rot, dirt, slime); others simply leave the game.

## Decay rate

Each item's decay rate is a data value: a multiplier set per item, or per category with per-item overrides. Organic items have multipliers that decay them over days; inorganic items have multipliers many times smaller. Values are set during balancing.

## Protection

Storage slows decay; it does not stop it (resolves OQ-027). Protection stacks in tiers:

| Storage | Effect on decay |
|---|---|
| Ground pool, unprotected | Full rate |
| Any container | Marginally slower |
| Container inside a shelter | Slower |
| Better containers: airtight, well designed, specialized (jars, urns, pots, cellars) | Slowest |

Exact rate multipliers are set during balancing.

## Food preservation

Food preservation is a core mechanic and one of the player's first goals, alongside a sustainable water supply ([GDD: early goals](../GameDesignDocument.md#early-goals)). Preservation methods are specified as crafting and research are designed.

## Removing inorganic items

Inorganic items decay very slowly, so in practice they leave a ground pool mainly when used or through extreme weather events ([Zones: ground pool](Zones.md#capacity)).

## Open questions

- Decay rates per item type are set during balancing.
