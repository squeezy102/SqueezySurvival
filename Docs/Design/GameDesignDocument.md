# Game Design Document

**Status:** Concept

## Vision

A graphically basic, strategically deep, single-player survival game. The player is a survivor stranded on a large island and must manage their body, mind and environment through deliberate, time-costed decisions.

Presentation is pictures, text, windows and progress bars. Depth comes from systems, not spectacle.

## Influences

| Game | What we take | What we improve |
|---|---|---|
| Card Survival: Tropical Island | Time-unit economy, every action has a cost, stat-driven survival, discovery-driven recipes | Replace the card UI with a clear sprite and inventory interface |
| Card Survival: Fantasy Forest | Same as above | Same as above |
| Songs of Syx | Systemic depth, readable management UI | - |
| Green Hell | Survival stat breadth, crafting from found materials | - |
| Ark: Survival Evolved | Progression through discovered technology | - |

## Pillars

1. **Everything is measured in time units.** Time advances only through actions. Every action declares its duration.
2. **Every action costs something.** Time at minimum; often also energy, stats, tool durability or materials. There are no free actions.
3. **Discovery drives progression.** Finding a resource for the first time unlocks research that uses it.
4. **Clarity over clutter.** The UI must make state, costs and consequences obvious. This is the primary differentiator from our influences.

## Presentation

- 2D sprites, text, windows and progress bars.
- **MVP: no animation.** All visual state changes are instant.
- Architecture must allow animation to be added later without changing game logic (presentation is decoupled from simulation).
- Prototype art: free-use sprites and placeholders from reputable open-source sources, with license attribution tracked.
- UI uses an inventory model, not cards.

## Core loop

```
Assess state -> Choose action -> Pay cost (time + other) -> Resolve outcome
     ^                                                            |
     +-------- Stats decay, weather changes, research progresses -+
```

1. The player reviews their condition, inventory, the current zone and its ground pool.
2. The player chooses an action. Its costs are shown before committing.
3. Time advances by the action's duration. Survival stats change accordingly.
4. Outcomes resolve: resources found, items crafted, stats restored or damaged.
5. Newly discovered resources unlock research options.

## World

- **Setting:** a large island.
- **Biomes:** each biome has its own resource set. The beach is the first and only MVP biome.
- **Zones:** discrete locations, each belonging to a biome. Zones connect to one another. Moving between zones is an action that costs time.
- **Zone resource pool:** exploring a zone unearths resources into that zone's pool ("on the ground"). Items in the pool stay in that zone and are not in the player's inventory.
- **Weather:** affects the player and the environment. Specifics in [Open Questions](OpenQuestions.md).

## Survivor

### Survival stats (concept)

| Stat | Concept |
|---|---|
| Hunger | Need for food |
| Thirst | Need for water |
| Satiation | Distinct from hunger - definition pending ([Open Questions](OpenQuestions.md)) |
| Sanity | Mental condition |
| Cleanliness | Filth level; the MVP start is "filthy" |

Stat ranges, decay rates and effects are not yet defined. Each will be specified in a system document before implementation.

### Equipment

Clothing provides protection. The MVP start is tattered clothing with minimal protection.

## Research

- The first time the player finds a resource, recipes that use it become available in the Research tab.
- Researching a recipe takes time and is passive. Whether it runs alongside other actions is pending ([Open Questions](OpenQuestions.md)).
- Completed research makes the recipe craftable.

## MVP scope

### Opening scenario

The player wakes on a beach.

| Condition | Starting value |
|---|---|
| Hunger | Half |
| Thirst | Half |
| Cleanliness | Filthy |
| Clothing | Tattered, minimal protection |
| Tools | None |

Initial actions:
- Wash off in the ocean.
- Explore the beach.

### Beach resources (initial list)

Coconuts, palm trees, crabs, shells, sand, sticks, rocks, big rocks. This list will grow.

### Early interactions (initial list)

| Interaction | Notes |
|---|---|
| Knap stone into a knife | First tool |
| Eat and drink coconuts | Food and water source |
| Eat crabs | Food source |
| Hand fishing | Possible but unreliable |

### Out of scope for MVP

- Animation.
- Multiple playable characters and starting locations (planned later).
- Biomes other than the beach.

## Distribution

A shareable, self-updating build is a primary deliverable from the first build onward. See [ADR-0004](../Decisions/ADR-0004-Distribution-And-Self-Update.md).
