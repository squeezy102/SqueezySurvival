# Actions

**Status:** Draft

Everything the survivor does is an action. This document defines what every action has and how availability is decided. Individual actions are specified in their owning systems.

## Anatomy of an action

| Part | Description |
|---|---|
| Duration | Whole minutes (TU), at least 1. See [Time](Time.md). |
| Costs | Anything consumed besides time: materials, stats, tool durability. |
| Requirements | Conditions that must hold to attempt it: tools, materials, location, light level, survivor condition. |
| Outcomes | What happens on success, and on failure where failure is possible. |
| Risks | Chances of side effects, such as injury, that depend on circumstances. |

The duration, costs and risks are shown before the player commits ([Pillar 4: clarity](../GameDesignDocument.md#pillars)).

## Availability

Every action is in one of three states. The UI always shows the state and the reason.

| State | Meaning | Example |
|---|---|---|
| Available | Can be performed normally. | Drink from a coconut in daylight. |
| Available with penalties | Can be performed, but with added time, failure chance or risk. The penalties are shown. | Forage in the dark: injury risk. |
| Unavailable | Cannot be performed. The blocking requirement is named. | Sleep while dangerously thirsty. |

## Pacing

**Proposal from squeezy102's notes (2026-10-19), not approved.** Every action should carry weight: today, clicking Explore over and over maps the Beach and fills the ground in seconds, and waiting doesn't feel like waiting.

- **An action takes real time.** While it runs, a small window in the middle of the screen names it ("Exploring the Beach") with a progress bar, and nothing else can be started.
- **Standard: 15 minutes of game time takes 1 second.** Other durations scale from it. Whether long actions are capped: [OQ-075](../OpenQuestions.md) (at this rate, a 1-hour Rest takes 4 seconds and an 8-hour sleep 32).
- **Later, small animations** replace the bar: dashed lines appearing one by one to an X for exploring, a bed giving off Zs for sleeping, a clock turning for waiting, a hammer striking an anvil for crafting. Never the survivor. Out of scope for the MVP ([GDD: presentation](../GameDesignDocument.md#presentation)); sound comes later too.

## Light level

Light level is a requirement and penalty source for many actions. It is derived from the day phase and modified by light sources.

| Light level | When | Effect on actions |
|---|---|---|
| Bright | Morning, Midday, Afternoon | No penalty. |
| Dim | Dawn, Dusk | Some actions gain penalties (time, failure chance, risk). |
| Dark | Night | Many actions are unavailable. Some remain available with penalties. |

Light sources raise the light level while active.

| Kind | Examples |
|---|---|
| Carried | The **primitive torch** (an early craftable: finicky, fragile and short-lived), candles |
| Placed | Campfires, fixed torches, candelabras |

Carried light goes with the survivor; placed light lights the zone or sub-zone it is in. Light sources are specified in their own system (planned).

### Acting in the dark

| Action type | In the dark |
|---|---|
| Wait, Rest | Always available. |
| Build a campfire | Available with penalties: longer duration; failure can waste materials. |
| Forage | Available with penalties: injury risk from trip and collision hazards (abrasions, scrapes, sprains). |
| Most other actions | Unavailable without a light source. Each action's specification states its light requirement. |

Injuries are specified in [Injuries and Health](Injuries.md).

## Condition gates

The survivor's condition can lock actions.

1. **Dangerous survival stats block sleep.** While any survival stat is Dangerous or Critical, Nap and Sleep are unavailable. Low stats also wear on the mind ([Survival Stats: bands drain the mind](SurvivalStats.md#survival-stat-bands-drain-the-mind)).
2. **Dangerous survival stats end sleep.** If a survival stat reaches a dangerous level during Nap or Sleep, the survivor is woken immediately, whatever the time. Time slept up to that moment counts; the remainder does not. The player is then faced with the problem in whatever light and weather the moment holds.
3. **Wait and Rest are always available**, so time can always be passed.

Band thresholds are specified in [Survival Stats](SurvivalStats.md#bands-draft).

## Design intent

A survivor caught unprepared at night should struggle: dark locks out most options, and the options that remain carry risk. Preparation (a fire before nightfall, a torch, water on hand) is rewarded. This tension is deliberate.
