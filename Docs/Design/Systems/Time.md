# Time

**Status:** Draft

Time is the game's core currency ([Pillar 1](../GameDesignDocument.md#pillars)). Every other system - actions, survival stats, research, weather - is measured in it.

## Rules

1. **Time advances only through player actions.** Nothing happens between actions. There is no real-time clock.
2. **The clock keeps time to the second, and shows it** (squeezy102, 2026-10-20, [#107](https://github.com/squeezy102/SqueezySurvival/issues/107)). Actions are authored in **time units (TU) of 1 minute**, which lets quick actions (drinking, eating, breaking a rock) cost realistically little without being free. A few tiny actions last whole seconds instead, such as moving things between the ground and the inventory ([Inventory: moving things](Inventory.md#moving-things)). No time is ever hidden: the clock always shows exactly where the run stands.
3. **Every action costs time** ([Pillar 2](../GameDesignDocument.md#pillars)): at least 1 TU, or at least 1 second for the tiny actions in rule 2.
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

### Sheltered variants

Wait, Rest, Nap and Sleep automatically become their sheltered versions when the survivor is sheltered: inside a sheltered [sub-zone](Zones.md#sub-zones) such as a cave, or in a zone with a shelter the player has built. The player never has to click the shelter to use it. Same durations; the survivor spends them protected from the [weather](Weather.md#weather-effects). Otherwise, they are spent in the open.

Design intent: in Card Survival, a player who forgets to click their shelter sleeps on the ground and suffers for it. Owning a shelter is what matters, not remembering to use it.

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
| Temperature | Coldest, rising | Warming | Hottest | Hot, easing | Cooling | Cold | [Weather](Weather.md) |
| Light level | Dim | Bright | Bright | Bright | Dim | Dark | [Actions](Actions.md#light-level) |
| Active wildlife | Crepuscular | Diurnal | Diurnal | Diurnal | Crepuscular | Nocturnal | [Encounters](Encounters.md) |
| Fishing catches | Phase-specific species tables | | | | | | Fishing (planned) |
| Foraging | Some plants available only in certain phases | | | | | | Foraging (planned) |

Crepuscular animals are active at dawn and dusk.

## Calendar

A year is **120 days**: four seasons of **30 days** each, in the order Temperate, Dry, Rainy, Stormy ([Weather: seasons](Weather.md#seasons)). A run starts on the first day of Temperate. (squeezy102, 2026-10-19.)

## Display

- Clock in 12-hour format with AM/PM and seconds (for example `7:45:20 AM`), in the HUD and in the event log.
- Day counter starting at Day 1.
- In the HUD, the day, the time and the day phase each have their own row, with no separators.
- Every action shows its duration before the player commits, as hours and minutes (for example `1 hr 30 min`), or as seconds for the tiny actions (`10 sec`).
- A **day and night dial** beside the clock ([#52](https://github.com/squeezy102/SqueezySurvival/issues/52)): a disc split in half, one half a bright day sky with the sun, the other a dim night sky with the moon and stars, seen through a half-circle window. The disc turns once a day with the clock: the sun is at the top at noon, the moon at midnight; at 6:00 AM the sun is rising on the left, and at 6:00 PM it is setting on the right. The disc is redrawn pixel by pixel at each angle, so it stays crisp pixel art as it turns ([Art Direction](../ArtDirection.md)).

## Implementation notes

- Game time is stored as a whole number of seconds elapsed since the start of the run. Clock and day are derived from it, never stored separately.
- Time advances only through one simulation entry point, so every time-dependent system sees the same duration in the same order. This keeps the simulation deterministic and unit testable.

