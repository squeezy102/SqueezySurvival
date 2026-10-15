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

## Light level

Light level is a requirement and penalty source for many actions. It is derived from the day phase and modified by light sources.

| Light level | When | Effect on actions |
|---|---|---|
| Bright | Morning, Midday, Afternoon | No penalty. |
| Dim | Dawn, Dusk | Some actions gain penalties (time, failure chance, risk). |
| Dark | Night | Many actions are unavailable. Some remain available with penalties. |

Light sources raise the light level for the survivor while active. The first is the **primitive torch**: an early craftable that is finicky, fragile and short-lived. A campfire is also a light source. Light sources are specified in their own system (planned).

### Acting in the dark

| Action type | In the dark |
|---|---|
| Wait, Rest | Always available. |
| Build a campfire | Available with penalties: longer duration; failure can waste materials. |
| Forage | Available with penalties: injury risk from trip and collision hazards (abrasions, scrapes, sprains). |
| Most other actions | Unavailable without a light source. Each action's specification states its light requirement. |

Injuries are specified in their own system (planned).

## Condition gates

The survivor's condition can lock actions.

1. **Dangerous survival stats block sleep.** While any survival stat is at a dangerous level, Nap and Sleep are unavailable and the survivor suffers ongoing sanity penalties.
2. **Dangerous survival stats end sleep.** If a survival stat reaches a dangerous level during Nap or Sleep, the survivor is woken immediately, whatever the time. Time slept up to that moment counts; the remainder does not. The player is then faced with the problem in whatever light and weather the moment holds.
3. **Wait and Rest are always available**, so time can always be passed.

What counts as "dangerous" for each stat, and the size of the sanity penalty, are specified in Survival Stats (planned).

## Design intent

A survivor caught unprepared at night should struggle: dark locks out most options, and the options that remain carry risk. Preparation (a fire before nightfall, a torch, water on hand) is rewarded. This tension is deliberate.
