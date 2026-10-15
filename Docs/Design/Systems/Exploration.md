# Exploration

**Status:** Draft

Exploring a zone is how the survivor finds resources, features and the way to other zones (resolves OQ-007).

## The Explore action

- **Duration: 15 TU** (a short walk).
- Exploring is always a worthwhile choice when the player has nothing better to do, and never carries a severe time cost.

## Model

Exploration combines two things:

1. **Progress:** each zone has an exploration percentage. Exploring raises it, uncovering the zone's fixed features. Significant discoveries happen at progress milestones.
2. **Finds:** every explore also draws loose items from the zone's find pool. Finds continue after the zone is fully explored.

## First explores are scripted

A run's first explores of the starting zone are fixed, not random, so every player can make their first tools.

| Requirement | Guarantee |
|---|---|
| Within the first couple of explores | Stones, sticks, and a fibrous plant (washed-up seaweed or similar) |
| Number of scripted explores | First 4-5 (exact number set with zone content) |
| After the scripted explores | Finds are drawn randomly from the zone's find pool |

## What exploring can reveal

| Type | Description | Starting zone examples |
|---|---|---|
| Loose items | Items placed in the zone's ground pool. | Stones, big rocks, sticks, seaweed, conches, crabs, coconuts, seashells, dead fish |
| Hazards | Dangers met while exploring; effects depend on the survivor's gear. | Washed-up jellyfish or sea urchins that sting bare feet. Unlikely in the starting zone. |
| Resource nodes | Semi-permanent features that can be harvested repeatedly. | Palm trees, tide pools, seagull nests, driftwood, a large carcass such as a beached turtle |
| Zone connections | The way into a connected zone. | A palm grove, a bay, a river inlet |

## Pace

Each zone defines how many explores it takes to map fully (resolves OQ-023). Each explore adds `100% / explores to map` progress.

| Zone size | Explores to map | Exploring time |
|---|---|---|
| Starting zone | 24 | 6 hours |
| Typical zones | 24-48 | 6-12 hours (half a day to a day of exploring) |

Time spent on everything else (crafting, eating, resting) spreads this across more game days.

## Milestones

Significant discoveries (resource nodes, zone connections) are tied to progress milestones, for example 25%, 50%, 75% and 100%. Exact milestones and what each reveals are set per zone as the world is designed.

## Open questions

[OQ-024](../OpenQuestions.md), [OQ-025](../OpenQuestions.md).
