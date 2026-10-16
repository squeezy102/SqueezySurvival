# Zones

**Status:** Draft

The world the survivor moves through.

## World

- **Setting:** a large island.
- **Biomes:** categories of environment, each with its own resource set. The beach is the first and only MVP biome.
- **Zones:** discrete locations, each belonging to one biome. Zones connect to one another through [zone connections](Exploration.md#what-exploring-can-reveal) found by exploring. Travelling to a connected zone is an action that costs time.
- **Weather** affects the survivor and the environment ([Weather](Weather.md)).

### Sub-zones

A **sub-zone** is a smaller place inside a zone, found by exploring it. A cave is the first sub-zone type.

- **Most caves are a single sub-zone**, like a cave mouth: shelter and little else.
- **Some caves can be explored.** Exploring them reveals [zone connections](Exploration.md#what-exploring-can-reveal) to deeper zones within. These are full zones: travelling to them costs time, and they have their own exploration progress, resources and encounters.

**Light in caves** ([Actions: light level](Actions.md#light-level)):

| Place | Light level |
|---|---|
| Cave mouth | One step darker than outside: Dim by day, Dark at Dusk and Night |
| Deeper cave zones | Always Dark |

A light source is needed for most actions deeper in a cave.

- **Moving between a zone and its sub-zones is free.** It is not an action and costs no time; it only changes where the survivor's next action happens. This is not an exception to [Pillar 2](../GameDesignDocument.md#pillars): no action is free, but changing place within a zone is not an action.
- A sub-zone has its own [ground pool](#ground-pool) and features, and can be furnished (a campfire, a leaf bed, storage).
- A cave is **sheltered**: everything done inside it counts as sheltered ([Time: sheltered variants](Time.md#sheltered-variants)).
- A cave has its own [encounter](Encounters.md) table. Spiders, centipedes and scorpions can bite or sting a survivor who rests or sleeps there.

The MVP island's size and layout: [OQ-008](../OpenQuestions.md).

## Ground pool

Each zone has a ground pool: the items lying on the ground there. Items enter it from [exploration](Exploration.md), harvesting [resource nodes](ResourceNodes.md), and anything the survivor puts down. Items in a zone's ground pool are not in the survivor's inventory and stay in that zone.

### Capacity

The ground pool has no capacity limit (resolves OQ-028). Items leave it only by:

| Mechanism | Affects |
|---|---|
| The survivor picking them up or using them | Anything |
| [Decay](Decay.md) | Everything; organic items within days, inorganic items only after a very long time |
| Extreme weather events (floods, storms, landslides and similar) and [wildfires](Weather.md#wildfire) | Anything |
