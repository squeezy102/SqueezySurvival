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

Wait and Rest are always available. Nap and Sleep are unavailable while any survival stat is dangerous, and a stat becoming dangerous ends them early (resolves OQ-018). See [Actions: condition gates](Actions.md#condition-gates).

What Rest, Nap and Sleep restore, and how they differ beyond duration, depends on survival stats ([OQ-004](../OpenQuestions.md)) and is specified there.

## Rates

Rates that change over time (stat decay, research progress, weather effects) are authored per hour, the unit designers reason in, and applied for the exact minutes an action takes. Values that can change by less than one whole point per minute are stored with fractional precision.

| Example action | Indicative cost |
|---|---|
| Drink | 1 TU |
| Eat a coconut | 5 TU |
| Break a rock | 10 TU |
| Skin a fish | 15 TU |
| Explore | 15 TU |

Indicative only; each action's cost is set in its own specification.

## Start of a run

A run starts on **Day 1 at 6:00 AM**, during Dawn: the survivor wakes on the beach with a full day ahead (resolves OQ-016).

## Day phases

The day is divided into six phases. Other systems read the current phase; none hard-codes clock times. Phase boundaries are data, so they can be tuned without code changes (resolves OQ-017).

| Phase | Starts | Ends | Length |
|---|---|---|---|
| Dawn | 5:00 AM | 7:00 AM | 2 hr |
| Morning | 7:00 AM | 11:00 AM | 4 hr |
| Midday | 11:00 AM | 2:00 PM | 3 hr |
| Afternoon | 2:00 PM | 6:00 PM | 4 hr |
| Dusk | 6:00 PM | 10:00 PM | 4 hr |
| Night | 10:00 PM | 5:00 AM | 7 hr |

### What phases affect

Direction of effect only. Each owning system specifies its own values.

| Affected | Dawn | Morning | Midday | Afternoon | Dusk | Night | Owner |
|---|---|---|---|---|---|---|---|
| Temperature | Coldest, rising | Warming | Hottest | Hot, easing | Cooling | Cold | Weather (planned) |
| Light level | Dim | Bright | Bright | Bright | Dim | Dark | [Actions](Actions.md#light-level) |
| Active wildlife | Crepuscular | Diurnal | Diurnal | Diurnal | Crepuscular | Nocturnal | [Encounters](Encounters.md) |
| Fishing catches | Phase-specific species tables | | | | | | Fishing (planned) |
| Foraging | Some plants available only in certain phases | | | | | | Foraging (planned) |

Crepuscular animals are active at dawn and dusk.

## Display

- Clock in 12-hour format with AM/PM (for example `7:45 AM`).
- Day counter starting at Day 1.
- Every action shows its duration before the player commits, as hours and minutes (for example `1 hr 30 min`).

## Implementation notes

- Game time is stored as a whole number of TU elapsed since the start of the run. Clock and day are derived from it, never stored separately.
- Time advances only through one simulation entry point, so every time-dependent system sees the same duration in the same order. This keeps the simulation deterministic and unit testable.

