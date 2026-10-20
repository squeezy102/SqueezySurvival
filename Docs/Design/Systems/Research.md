# Research

**Status:** Draft

Research turns discovered knowledge into craftable recipes and other unlocks.

## Rules

1. **Research is passive.** It progresses while the survivor does anything else and never blocks or interrupts an action (resolves OQ-005).
2. **Its only cost is time.** Research progresses by the duration of every action ([Time](Time.md#rules)). The player's job is to keep something researching.
3. **Completed research grants recipes**, including blueprints for structures, which become craftable.
4. **Knowledge is never lost** within a run. Discoveries, topics and recipes stay known even if the items behind them are used up, [decay](Decay.md) or break: a survivor whose last crucible breaks still knows how to make one.

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

**Content rule:** chains are authored so that discovering a material means the player already has what its topics' recipes need. A copper bar can only be made with a forge, so by the time copper tool topics appear, the player has one.

## Idle research

Having nothing researching wastes time, so the game says so:

- **Nav bar:** the research icon is highlighted while nothing is researching or queued. The MVP uses a static highlight (icon and border, never color alone); a bounce or pulse arrives with animation, together with the reduce-motion setting ([HUD: event log](../UI/HUD.md#event-log)).
- **Idle icon:** an atom with "zzz" in the [HUD](../UI/HUD.md) static status area while nothing is researching. Hovering explains it: "No research queued."
- **Event log:** "Your research is complete. Your mind is idle." when the last topic finishes.

## Proposals (not approved)

From squeezy102's notes (2026-10-19), for discussion.

- **What is researched:** single things (a stone axe, a campfire, a clay vessel) or whole fields (stone tools, clay forming, firemaking)? [OQ-076](../OpenQuestions.md).
- **Speeding research up with materials.** A topic takes its time, or the player can hand in its listed materials (for example 5 stones, 5 sticks and 5 cords) to cut its **total** time in half; with less than half left, it completes at once. Only the full list counts. This changes rule 2 above ("its only cost is time", OQ-005): [OQ-077](../OpenQuestions.md).
- **Finding things in the window.** Sort alphabetically, by time, or by materials on hand, ascending or descending. Topics are grouped by category, each opening a list beside it, like the old Windows Start menu. There will be many recipes; the window does the organizing, never the player.
- **New topics are announced** in the HUD, in the research window, in the event log ("New research unlocked: Ceramics"), perhaps with a pop-up, and the player can always see where in the window a new topic is.
- **Layout.** The research window covers the zone features panel, like the other windows from the nav bar, and leaves the ground and inventory usable. To be designed in a dedicated session.

