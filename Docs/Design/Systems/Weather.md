# Weather

**Status:** Draft

Weather affects the survivor and the environment. Each zone has a current weather state, shown as an icon in the [HUD](../UI/HUD.md).

## Weather states

Ten states, from mildest to most severe (resolves OQ-009). There are no tornadoes.

| State | Category |
|---|---|
| Sunny | Normal |
| Partly cloudy | Normal |
| Cloudy | Normal |
| Foggy | Normal |
| Windy | Normal |
| Rainy | Normal |
| Heavy rain | Normal |
| Storm | Normal |
| Heavy storm | Severe |
| Hurricane | Disaster |

What each state does to stats, actions, temperature and the environment, and how weather changes over time: [OQ-052](../OpenQuestions.md).

## Seasons

The year has seasons, including a **dry season**, which makes wildfires possible. Which seasons exist and how long they last: [OQ-053](../OpenQuestions.md).

## Wildfire

A wildfire is a disaster the player causes by accident.

### Cause

All of these must be true:

| Condition | Detail |
|---|---|
| A fire is left unattended | The survivor starts a full Sleep action while a fire burns in the zone, or leaves the zone while a fire burns there. |
| It is the dry season | See [Seasons](#seasons). |
| The zone has plenty of fuel | Many semi-permanent or permanent bush and foliage features, and bush and foliage resources lying in the [ground pool](Zones.md#ground-pool). |

### Effects

- **The survivor must leave the zone** and cannot return until the fire has burned out.
- **Organic resources** in the zone are turned to charcoal or destroyed outright.
- **Structures** in the zone are damaged and need repairs, or are destroyed.

Wildfire chance, how long the zone stays closed, how much is lost, and what happens to a survivor asleep in the zone when it starts: [OQ-054](../OpenQuestions.md).
