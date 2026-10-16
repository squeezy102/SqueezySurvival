# Decay

**Status:** Draft

Decay is a resource sink: organic items that are not used or protected deteriorate over time.

## What decays

Organic items lying in a zone's ground pool, or not well protected by storage, deteriorate over time. Inorganic materials (stone, shells, metal) do not decay (resolves OQ-026).

| Item type | Examples | Result when fully decayed |
|---|---|---|
| Food | Fish, fruit, meat | Converts: rot, slime |
| Plant matter | Leaves, seaweed | Converts: dried leaves, then rot or dirt |
| Wood | Sticks, logs | Removed |
| Crafted items | Tools, torches | Breaks; leaves only part of its materials, usually not the important ones |

Some results are themselves useful resources (rot, dirt, slime); others simply leave the game.

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

Inorganic items do not decay, so another mechanic must remove them from ground pools when a sink is needed. On the beach, the tide can bury or wash away items. Other zones (forests, mountains) need their own mechanic ([OQ-028](../OpenQuestions.md)).

## Open questions

- Decay rates per item type are set during balancing.
- [OQ-028](../OpenQuestions.md).
