# Zones

**Status:** Draft

The world the survivor moves through.

## World

- **Setting:** a large island.
- **Biomes:** categories of environment, each with its own resource set. Biomes are a core concept: every zone belongs to one. The MVP has three: Coast, River and Jungle; more follow.
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

### MVP zones

| Zone | Biome | Character |
|---|---|---|
| Beach | Coast | The starting zone. |
| Cliff | Coast | Rocky coastal heights. |
| Estuary | Coast | Where the river meets the sea. Brackish water: fresh and salt water mixed ([NOAA: what is an estuary](https://oceanservice.noaa.gov/facts/estuary.html)), so not safe to drink. Tidal mudflats. |
| Riverbank | River | Upstream of the estuary: fresh water. |
| Palm grove | Jungle | Palm trees inland from the beach. |
| Jungle thicket | Jungle | Dense jungle beyond the riverbank. |

Connections (resolves OQ-008):

```
Cliff --- Beach --- Palm grove
            |
         Estuary --- Riverbank --- Jungle thicket
```

The Beach is the hub. Fresh water at the Riverbank is two zones from the start, so securing a reliable water supply, an early goal ([GDD: early goals](../GameDesignDocument.md#early-goals)), takes a journey.

## Ground pool

Each zone has a ground pool: the items lying on the ground there. Items enter it from [exploration](Exploration.md), harvesting [resource nodes](ResourceNodes.md), and anything the survivor puts down. Items in a zone's ground pool are not in the survivor's inventory and stay in that zone.

### Capacity

The ground pool has no weight limit (resolves OQ-028): weight on the ground is shown for information only. It does have a fixed number of slots, one kind of item per slot, shown as the ground's [inventory grid](../UI/InventoryGrids.md#the-grids). When every slot is taken and a new kind arrives, the player chooses what to throw away ([Inventory Grids: when the ground is full](../UI/InventoryGrids.md#when-the-ground-is-full)).

Items leave it only by:

| Mechanism | Affects |
|---|---|
| The survivor picking them up or using them | Anything |
| [Decay](Decay.md) | Everything; organic items within days, inorganic items only after a very long time |
| Extreme weather events (floods, storms, landslides and similar) and [wildfires](Weather.md#wildfire) | Anything |
