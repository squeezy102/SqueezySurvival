# Time

**Status:** Draft

Time is the game's core currency ([Pillar 1](../GameDesignDocument.md#pillars)). Every other system - actions, survival stats, research, weather - is measured in it.

## Rules

1. **Time advances only through player actions.** Nothing happens between actions. There is no real-time clock.
2. **The time unit (TU) is 1 minute of game time.** It is the smallest unit of time; no duration is fractional. Minute resolution lets quick actions (drinking, eating, breaking a rock) cost realistically little without being free.
3. **Every action costs at least 1 TU** ([Pillar 2](../GameDesignDocument.md#pillars)).
4. **A day is 1,440 TU** (24 hours x 60 minutes).
5. **When an action resolves, all time-dependent systems advance by its full duration** (stat changes, research progress, weather).

## Passing time deliberately

| Action | Duration | Game time |
|---|---|---|
| Wait | 15 TU | 15 minutes |
| Rest | 60 TU | 1 hour |
| Nap | 240 TU | 4 hours |
| Sleep | 480 TU | 8 hours |

What Rest, Nap and Sleep restore, and how they differ beyond duration, depends on survival stats ([OQ-004](../OpenQuestions.md)) and is specified there.

## Rates

Rates that change over time (stat decay, research progress, weather effects) are authored per hour, the unit designers reason in, and applied for the exact minutes an action takes. Values that can change by less than one whole point per minute are stored with fractional precision.

| Example action | Indicative cost |
|---|---|
| Drink | 1 TU |
| Eat a coconut | 5 TU |
| Break a rock | 10 TU |
| Skin a fish | 15 TU |
| Explore an area | 60 TU |

Indicative only; each action's cost is set in its own specification.

## Display

- Clock in 12-hour format with AM/PM (for example `7:45 AM`).
- Day counter starting at Day 1.
- Every action shows its duration before the player commits, as hours and minutes (for example `1 hr 30 min`).

## Implementation notes

- Game time is stored as a whole number of TU elapsed since the start of the run. Clock and day are derived from it, never stored separately.
- Time advances only through one simulation entry point, so every time-dependent system sees the same duration in the same order. This keeps the simulation deterministic and unit testable.

## Open questions

[OQ-016](../OpenQuestions.md), [OQ-017](../OpenQuestions.md), [OQ-018](../OpenQuestions.md).
