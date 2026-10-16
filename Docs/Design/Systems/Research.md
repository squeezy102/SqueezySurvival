# Research

**Status:** Draft

Research turns discovered knowledge into craftable recipes and other unlocks.

## Rules

1. **Research is passive.** It progresses while the survivor does anything else and never blocks or interrupts an action (resolves OQ-005).
2. **Its only cost is time.** Research progresses by the duration of every action ([Time](Time.md#rules)). The player's job is to keep something researching.
3. **Completed research grants recipes**, including blueprints for structures, which become craftable.

## Queue

- One topic is researched at a time.
- Up to 3 more topics can wait in a queue. When the current topic completes, the next one starts automatically (resolves OQ-006).
- The player can reorder or remove queued topics at any time.

## Availability

**Research topics are unlocked by discovering a material. Nothing else unlocks a topic** (resolves OQ-046). There are no prerequisite topics, building requirements or tool requirements on topics, so no topic is ever shown locked: a topic appears the moment its material is discovered ([You only know what you know](../GameDesignDocument.md#pillars)).

Progression chains form naturally, because crafted materials are discoveries too:

| Discovery | Unlocks topic | Topic grants |
|---|---|---|
| Copper ore | Refining and smelting | Recipes for a forge, tongs and a crucible |
| Copper bar (smelted in a crucible and cooled) | Copper tools, copper weapons, copper armor | Recipes for each |

Requirements such as needing a forge belong to the recipes a topic grants, not to the topic.

## Idle research

Having nothing researching wastes time, so the game says so:

- **Nav bar:** the research icon is highlighted while nothing is researching or queued. The MVP uses a static highlight (icon and border, never color alone); a bounce or pulse arrives with animation, together with the reduce-motion setting ([HUD: event log](../UI/HUD.md#event-log)).
- **Idle icon:** an atom with "zzz" in the [HUD](../UI/HUD.md) static status area while nothing is researching. Hovering explains it: "No research queued."
- **Event log:** "Your research is complete. Your mind is idle." when the last topic finishes.
