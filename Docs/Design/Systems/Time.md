# Time

**Status:** Draft

Time is the game's core currency ([Pillar 1](../GameDesignDocument.md#pillars)). Every other system - actions, survival stats, research, weather - is measured in it.

## Rules

1. **Time advances only through player actions.** Nothing happens between actions. There is no real-time clock.
2. **The time unit (TU) is 15 minutes of game time.** It is the smallest unit of time; no duration is fractional.
3. **Every action costs at least 1 TU** ([Pillar 2](../GameDesignDocument.md#pillars)).
4. **A day is 96 TU** (24 hours x 4 TU per hour).
5. **When an action resolves, all time-dependent systems advance by its full duration** (stat changes, research progress, weather).

## Passing time deliberately

| Action | Duration | Game time |
|---|---|---|
| Wait | 1 TU | 15 minutes |
| Rest | 4 TU | 1 hour |
| Nap | 16 TU | 4 hours |
| Sleep | 32 TU | 8 hours |

What Rest, Nap and Sleep restore, and how they differ beyond duration, depends on survival stats ([OQ-004](../OpenQuestions.md)) and is specified there.

## Display

- Clock in 12-hour format with AM/PM (for example `7:45 AM`).
- Day counter starting at Day 1.
- Every action shows its duration before the player commits, as hours and minutes (for example `1 hr 30 min`).

## Implementation notes

- Game time is stored as a whole number of TU elapsed since the start of the run. Clock and day are derived from it, never stored separately.
- Time advances only through one simulation entry point, so every time-dependent system sees the same duration in the same order. This keeps the simulation deterministic and unit testable.

## Open questions

[OQ-016](../OpenQuestions.md), [OQ-017](../OpenQuestions.md), [OQ-018](../OpenQuestions.md).
