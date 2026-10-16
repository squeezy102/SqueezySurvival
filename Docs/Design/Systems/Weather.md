# Weather

**Status:** Draft

Weather affects the survivor and the environment. **Weather is island-wide:** every zone has the same weather state at the same time, shown as an icon in the [HUD](../UI/HUD.md). Local features change how much of it reaches the survivor (forest canopy, caves, shelters), never the state itself.

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

## Weather effects

Effects below are the design intent. Magnitudes and how weather changes over time: [OQ-052](../OpenQuestions.md). Every effect that happens to the survivor or their things is reported in the [event log](../UI/HUD.md#event-log).

### Clear and fair weather

| State | Effects |
|---|---|
| Sunny | **Sunburn** on long exposure. Prevented by applying mud or aloe, or by spending time in shelter now and then. Reduced in forested zones with canopy cover. |
| Partly cloudy | Sunburn risk as Sunny, but lower. |
| Cloudy | Little to no sunburn risk. Good weather for long outdoor work such as fishing or building. |

### Fog

- **Visibility is limited**, raising the risk of tripping and scraping yourself.
- **Ambushes are more likely** ([Encounters: ambushes](Encounters.md#ambushes)).
- **Exploration finds fewer permanent and semi-permanent features** ([Exploration](Exploration.md)).
- **On cold days**, fog raises the risk of hypothermia and makes the survivor wet and uncomfortable.

### Wind

- **Fires burn faster and hotter.**
- **Birds are more active** ([Encounters](Encounters.md)).
- **Loose light items can blow away** from the ground pool and from exposed storage: leaves, fibers, scraps of cloth, ash and similar. The survivor is notified when this happens.
- **Eye irritation** in dirty or sandy zones.
- **Temperatures are generally cooler.**

### Rain and storms

Each state includes everything in the state before it, more strongly.

| Effect | Rainy | Heavy rain | Storm | Heavy storm | Hurricane |
|---|---|---|---|---|---|
| Survivor gets wet when exposed | Yes | Yes | Yes | Yes | Yes |
| Puddles, tide pools and rain-catching containers fill ([Resource Nodes](ResourceNodes.md#replenishment)) | Slowly | Faster | Fast | Fast | Fast |
| Exposed fires go out | Sometimes | Often | Very hard to keep lit unless sheltered | Very hard unless sheltered | Impossible unsheltered |
| Dirt turns to mud | Slowly | Faster | Fast | Fast | Fast |
| Exploring and travelling | Mildly hampered | Hampered | Hard | Very hard; not recommended | Impossible |
| Tool work: abrasions and mistakes | Raised | Higher | High | Very high | Foolish |
| Climbing risk | Raised | Higher | High | Very high | Idiocy |
| Visibility | - | Reduced | Obscured | Badly obscured | Near zero |
| Items blow away | - | - | Some | Definitely | Large quantities |
| Temperature | - | - | Can become dangerously cold | Dangerously cold | Plummets |
| Structure damage | - | - | Possible, not probable | About 50% | Certain |
| Flying debris | - | - | - | - | Potentially life-threatening injuries |

- **Rainwater is safe to drink.** Containers left out catch it.
- **Storms:** the survivor should strongly consider sheltering in place unless they have important business.
- **Hurricanes** can last several days. A survivor without a safe shelter will almost surely die, and one without a stockpile of clean water and food will struggle to wait it out. Attempting almost anything during a hurricane is foolish.

## Seasons

Seasons follow a fixed order every year (resolves OQ-053, lengths to be tuned):

| Season | Character |
|---|---|
| Temperate | Mild and mixed. A run starts here, giving the survivor a gentle first week or two. |
| Dry | Hot, little rain. Water runs short; wildfires are possible. |
| Rainy | Frequent rain, building toward storms. |
| Stormy | Cold and stormy; when severe storms and hurricanes are most likely. |

Then Temperate again, and the cycle repeats. The wet and stormy part of the year arrives around day 30 (draft). A season sets which weather is likely; it never forces a state.

## How weather changes

Weather is dynamic but follows intuitive patterns, so the player can read it and plan ([Pillar 4: clarity](../GameDesignDocument.md#pillars)).

1. **Weather never jumps.** It moves one step at a time to a neighboring state:

```
Sunny - Partly cloudy - Cloudy - Rainy - Heavy rain - Storm - Heavy storm - Hurricane
                          |
                     Foggy, Windy
```

   Foggy and Windy branch off Cloudy. There is never a heavy storm without a storm first, never a hurricane without a heavy storm first, and Sunny passes through Partly cloudy on the way to Cloudy.
2. **The season weights each step.** In the Rainy season, steps toward rain are more likely; in the Dry season, steps toward Sunny are. Over days this produces trends the player can feel: "It has been raining a lot lately, and the storms are getting worse," or "It hasn't rained in days. I should conserve water."
3. **Every state lasts a while** before it can change, so weather never flickers.

This is a Markov chain over the states above, with transition weights set per season. Step weights and minimum durations: [OQ-052](../OpenQuestions.md).

## Wildfire

A wildfire is a disaster the player causes by accident.

### Cause

All of these must be true:

| Condition | Detail |
|---|---|
| A fire is left unattended | The survivor starts a full Sleep action while a fire burns in the zone, or spends **an hour or more** in another zone while a fire burns there. A shorter trip away does not count. |
| It is the Dry season | See [Seasons](#seasons). |
| The zone has plenty of fuel | Many semi-permanent or permanent bush and foliage features, and bush and foliage resources lying in the [ground pool](Zones.md#ground-pool). |

### Effects

- **The zone burns for a while.** Being in it while it burns is dangerous ([Caught in a wildfire](#caught-in-a-wildfire)).
- **A wildfire never spreads to another zone.** One mistake costs one zone, never the island.
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
