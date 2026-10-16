# Research

**Status:** Draft

Research turns discovered knowledge into craftable recipes and other unlocks.

## Rules

1. **Research is passive.** It progresses while the survivor does anything else and never blocks or interrupts an action (resolves OQ-005).
2. **Its only cost is time.** Research progresses by the duration of every action ([Time](Time.md#rules)). The player's job is to keep something researching.
3. **Completed research unlocks its result**, usually a recipe that becomes craftable.

## Queue

- One topic is researched at a time.
- Up to 3 more topics can wait in a queue. When the current topic completes, the next one starts automatically (resolves OQ-006).
- The player can reorder or remove queued topics at any time.

## Availability

A topic becomes available only when all its requirements are met:

| Requirement | Example |
|---|---|
| A discovered resource | Finding a sharp stone for the first time makes stone knapping researchable ([Pillar 3](../GameDesignDocument.md#pillars)). |
| Prerequisite research | A topic depends on one or more completed topics. |
| A built structure | A topic requires a building, such as a shelter or a kiln. |
| A crafted tool | A topic requires a tool, such as a flint knife. |

Whether a topic with unmet requirements is shown: [OQ-046](../OpenQuestions.md).

## Idle research

Having nothing researching wastes time, so the game says so:

- **Nav bar:** the research icon is highlighted while nothing is researching or queued. The MVP uses a static highlight (icon and border, never color alone); a bounce or pulse arrives with animation, together with the reduce-motion setting ([HUD: event log](../UI/HUD.md#event-log)).
- **Idle icon:** an atom with "zzz" in the [HUD](../UI/HUD.md) static status area while nothing is researching. Hovering explains it: "No research queued."
- **Event log:** "Your research is complete. Your mind is idle." when the last topic finishes.
