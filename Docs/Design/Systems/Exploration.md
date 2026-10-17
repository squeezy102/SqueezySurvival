# Exploration

**Status:** Draft

Exploring a zone is how the survivor finds resources, features and the way to other zones (resolves OQ-007).

## The Explore action

- **Duration: 15 TU** (a short walk).
- Exploring is always a worthwhile choice when the player has nothing better to do, and never carries a severe time cost.
- Finds are unlimited. The limit is the survivor: every explore taxes time, stamina, food, water, exposure to the elements and the risk of hazards.

## Model

Exploration combines two things:

1. **Progress:** each zone has an exploration percentage. Exploring raises it, uncovering the zone's fixed features. Significant discoveries happen at progress milestones.
2. **Finds:** every explore also draws loose items from the zone's find pool. Exploring a fully explored zone still draws finds, indefinitely: loose items never run out.

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
| Resource nodes | Fixed features harvested with actions; some replenish, some are consumed. See [Resource Nodes](ResourceNodes.md). | Palm trees, tide pools, seagull nests, driftwood, a large carcass such as a beached turtle |
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

## Wireframe stage

The first playable version of exploring, built as the real system with placeholder content so pacing can be tried and tuned ([#3](https://github.com/squeezy102/SqueezySurvival/issues/3)).

| Rule | Wireframe value |
|---|---|
| Duration | 15 TU |
| Progress | The Beach takes 24 explores to map, so each explore adds about 4%. Shown rounded down, so 100% appears only when the zone is fully mapped. |
| Scripted explores | The Beach's first 4: 2 stones and a stick; 2 sticks and a strand of seaweed; a strand of seaweed and a seashell; a stone and a coconut. No features. |
| Random finds | 1 to 3 loose items per explore, drawn from the zone's find pool by weight. Continues forever, also after the zone is fully explored. |
| Features | Each random explore of a zone not yet fully explored has a 25% chance to reveal one feature, drawn from the zone's feature pool by weight. A fully explored zone reveals no new features. |
| Event log | "You explored the Beach and found 2 stones and a stick." A revealed feature adds "You came across a tide pool." Mapping the zone fully adds a line saying so. |

Placeholder Beach content, taken from the examples above; weights are placeholders:

| Find pool (weight) | Feature pool (weight) |
|---|---|
| Stone (3), Stick (3), Seaweed strand (2), Seashell (2), Conch (1), Coconut (1) | Small palm tree (3), Medium palm tree (2), Large palm tree (1), Bush (2), Rock outcrop (1), Tide pool (1), Driftwood pile (2, semi-permanent), Puddle (2, semi-permanent) |

Random features stand in for [milestones](#milestones) until zone content is designed. Found items and features can't be used yet: picking up, harvesting and zone connections come later.

## Implementation notes

- An explore is resolved by `ZoneExplorer` in `SqueezySurvival.Core.Exploration`; the numbers live in `ExplorationRules`, and placeholder content in `PlaceholderBeachZoneContent`.
- Randomness comes from an injected `IRandomNumberSource`, so tests supply a fixed sequence ([Coding Standards: architecture](../../Engineering/CodingStandards.md#architecture)).
- Progress is stored as explores completed; the percentage is derived from it.

## Open questions

[OQ-025](../OpenQuestions.md).
