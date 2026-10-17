# Game Design Document

**Status:** Concept

## Vision

A graphically basic, strategically deep, single-player survival game. The player is a survivor stranded on a large island and must manage their body, mind and environment through deliberate, time-costed decisions.

Presentation is pictures, text, windows and progress bars. Depth comes from systems, not spectacle.

## Influences

| Game | What we take | What we improve |
|---|---|---|
| Card Survival: Tropical Island and Card Survival: Fantasy Forest | Time-unit economy, every action has a cost, stat-driven survival, discovery-driven recipes | Replace the card UI with a clear sprite and inventory interface |
| Songs of Syx | Systemic depth and a readable management UI, including some of its UI elements. Later in development: different survivors (races, starting characters) with their own food, housing and routine-activity preferences | - |
| Green Hell | Survival stat breadth, crafting from found materials, and its negative scenarios: the sense that if you are not careful, everything is trying to kill you | - |
| Ark: Survival Evolved | Research driven by discovery: finding a material opens research topics, and completing a topic grants lasting knowledge (recipes and blueprints) that is never lost during the run ([Research](Systems/Research.md)) | - |
| Haven and Hearth | Much of the survivor's body and knowledge: two-layer health (soft and hard HP, our current health and injury ceiling); injuries, healing and escalation; first aid; treatments that cost equipment slots; the equipment slot layout; "you only know what you know"; and item discovery unlocking new research and crafting | Adapted to a single-player, click-driven game |
| Pokémon (classic) and EarthBound | Battle presentation: encounters open a separate battle scene; the opponent at the top right and the player's side at the bottom left (Pokémon); the survivor never shown in battle (EarthBound); text boxes that announce each action and wait for the player ([Combat: presentation](Systems/Combat.md#presentation)). Attack, item and ability animations arrive after the MVP ([Presentation](#presentation)) | - |

## Pillars

1. **Everything is measured in time units.** Time advances only through actions. Every action declares its duration. See [Time](Systems/Time.md).
2. **Every action costs something.** Time at minimum; often also energy, stats, tool durability or materials. There are no free actions.
3. **Discovery drives progression.** Finding a resource for the first time unlocks research that uses it.
   - **You only know what you know.** The game never shows information about anything the survivor has not encountered: no hidden foods in lists, no undiscovered resources in tables, no unknown recipes. Knowledge is earned through play and appears as it is gained.
4. **Clarity over clutter.** The UI must make state, costs and consequences obvious. This is the primary differentiator from our influences.
   - Bars and meters always read "higher is better".
   - Iconography shows meaning everywhere, for example end-cap icons on every bar showing what empty and full mean. The player should never have to wonder what is good and what is bad.
   - **Color is never the only signal.** Every state shown by color is also shown by shape, icon, fill level, pattern or text, so the game is fully playable with color blindness. UI palettes are checked against red-green and blue-yellow color blindness before release.
   - **Sound is never the only signal.** Every event conveyed by sound is also shown on screen.
   - **Accessibility baseline:** the Basic tier of the [Game Accessibility Guidelines](https://gameaccessibilityguidelines.com/full-list/), which includes both rules above ("Ensure no essential information is conveyed by a colour alone" and "...by sounds alone").

## Design principles

- **Reward, don't punish.** Behavior we want to encourage (variety, rest, exploration) is rewarded rather than its absence penalized. Players respond far better to a bonus than to a mechanically equivalent penalty: World of Warcraft's beta XP penalty for long sessions was hated, while the same system reframed as a rested bonus was welcomed ([Psychology of Games](https://www.psychologyofgames.com/2010/03/framing-and-world-of-warcrafts-rest-system/)). Hard consequences remain where survival demands them (starvation, injury, illness).

- **Nothing happens until you let go.** Any action started by dragging, holding or aiming shows its outcome before it is committed, and is committed only on release; releasing where nothing would happen changes nothing. The first use is [drag and drop](UI/DragAndDrop.md).

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

A large island of connected zones, each with a ground pool of items lying there. See [Zones](Systems/Zones.md), [Exploration](Systems/Exploration.md), [Resource Nodes](Systems/ResourceNodes.md) and [Encounters](Systems/Encounters.md).

## Long-term vision

The island grows highly diverse over time: temperate forest, tropical jungle, desert, an arctic hellscape, a thriving atoll and more. Version 1.0 delivers a reasonable set of biomes; the rest are added in later development. The MVP starts with Coast, River and Jungle ([Zones](Systems/Zones.md#mvp-zones)).

## Survivor

The survivor has **no gender**, and one is never specified. The sprite is androgynous. All game text addresses the player as "you" (as the [event log](UI/HUD.md#event-log) already does) or refers to "the survivor", never with gendered words.

The player chooses the survivor's [dominant hand](Systems/Equipment.md#handedness) at character select.

The survivor's body and mind are specified in [Survival Stats](Systems/SurvivalStats.md), [Food](Systems/Food.md), [Exertion](Systems/Exertion.md), [Injuries and Health](Systems/Injuries.md), [Equipment](Systems/Equipment.md) and [Skills](Systems/Skills.md).

## Runs

Death ends the run and deletes its save; there is no save scumming. Casual mode (free saving, no points) and easy mode (easier, 1/5 survival points, no challenge points) are optional, as is hard mode (harder, double points). Each run earns points that persist and buy perks or challenges for later runs. See [Runs](Systems/Runs.md).

## Research

Passive, time-based unlocking of recipes, queued up to 3 topics ahead. See [Research](Systems/Research.md).

## MVP scope

### Opening scenario

The player wakes on a beach.

| Condition | Starting value |
|---|---|
| Food | Half |
| Water | Half |
| Cleanliness | Low: filthy |
| Clothing | Tattered, minimal protection |
| Tools | None |

Initial actions:
- Wash off in the ocean.
- Explore the beach.

### Beach resources (initial list)

Coconuts, palm trees, crabs, shells, sand, sticks, rocks, big rocks. This list will grow.

Food sources: coconuts, bananas, conches, crabs, seagulls, bugs, seeds, bushes and herbs.

**Content rule:** the starting zone always offers enough food variety, across several categories, that a player is never forced into a diet that food tolerance punishes ([Food](Systems/Food.md)).

### Early interactions (initial list)

| Interaction | Notes |
|---|---|
| Knap stone into a knife | First tool |
| Eat and drink coconuts | Food and water source |
| Eat crabs | Food source |
| Hand fishing | Possible but unreliable |

### Early goals

The first things a player should work towards:

1. **Sustainable water:** a reliable source of safe drinking water.
2. **Food preservation:** keeping food from spoiling ([Decay](Systems/Decay.md)).

### Out of scope for MVP

- Animation.
- Multiple playable characters and starting locations (planned later).
- Biomes other than Coast, River and Jungle ([Zones: MVP zones](Systems/Zones.md#mvp-zones)).

## Distribution

A shareable, self-updating build is a primary deliverable from the first build onward. See [ADR-0004](../Decisions/ADR-0004-Distribution-And-Self-Update.md).
