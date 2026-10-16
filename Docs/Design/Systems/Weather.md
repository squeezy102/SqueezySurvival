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
| A fire is left unattended | The survivor starts a full Sleep action while a fire burns in the zone, or spends **an hour or more** in another zone while a fire burns there. A shorter trip away does not count. |
| It is the dry season | See [Seasons](#seasons). |
| The zone has plenty of fuel | Many semi-permanent or permanent bush and foliage features, and bush and foliage resources lying in the [ground pool](Zones.md#ground-pool). |

### Effects

- **The zone burns for a while.** Being in it while it burns is dangerous ([Caught in a wildfire](#caught-in-a-wildfire)).
- **Damage spreads over time.** From the moment the fire starts, items and structures are progressively charred, damaged or destroyed. The longer it burns, the more is lost.
- **Organic resources** in the zone are turned to charcoal or destroyed outright.
- **Structures** in the zone are damaged and need repairs, or are destroyed.

### Caught in a wildfire

The survivor is caught in a wildfire in two ways:

- **Waking to one:** the fire was left burning during Sleep. The survivor wakes immediately, like any other sleep interruption ([Actions: condition gates](Actions.md#condition-gates)).
- **Entering one:** the survivor returns to, or travels into, a zone that is burning. When a wildfire starts in a zone the survivor is not in, the [event log](../UI/HUD.md#event-log) gives a warning such as "You see smoke on the horizon..." and the zone is marked as burning. The zone is shown as burning before they commit to travelling ([Pillar 4: clarity](../GameDesignDocument.md#pillars)).

**Rush back or wait it out.** A survivor away from a burning zone chooses: go back in and salvage what they can before more burns, or wait for the fire to burn out and accept the losses. Entering a burning zone is a high-risk action, and its risk is shown before committing.

Either way, once inside:

1. **Escape window: 15-30 minutes** of game time to grab what they can and get out. Picking up items and other actions spend that time as usual.
2. **Staying too long is dangerous**, and gets worse the longer the survivor stays: burns, smoke inhalation, overheating, dehydration, and finally death.
3. **Leaving** means travelling to a connected zone. Whatever is left behind is exposed to the fire.

Wildfire chance, how long the zone stays closed, how much is lost, the exact escape window, and how fast harm escalates past it: [OQ-054](../OpenQuestions.md).
