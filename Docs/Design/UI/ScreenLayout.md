# Screen Layout

**Status:** Draft

The main gameplay screen. Every region follows [Pillar 4: clarity](../GameDesignDocument.md#pillars) and the [Controls](Controls.md) spec.

## Wireframe

```
+---------------------------------------------------------------------+-+
| HUD             | Current zone - [Explore] - progress 35%           |N|
| macros, temps,  | Connected zones: [Palm grove] [Bay]               |a|
| stamina, load   +---------------------------------------------------|v|
|                 |                                                   | |
|                 |     Zone features (permanent / semi-permanent)    |b|
|                 |                                                   |a|
|          Scene art of the current zone (background)                 |r|
|                 |                                                   | |
|                 |     Ground pool (items on the ground)             | |
| Event log       |                                                   | |
| (lower left)    |                                                   | |
+-----------------+---------------------------------------------------+-+
| Inventory (collapsible)                                               |
+-----------------------------------------------------------------------+
```

## Regions

| Region | Position | Contents |
|---|---|---|
| Scene | Background, largest area | Art depicting the current zone. May change on each explore to suggest shifting scenery. |
| Zone bar | Top | Current zone name, the zones reachable from it, the Explore button, and the zone's exploration progress. Travelling is a click on a connected zone. |
| HUD | Top left corner | Survival stats, outside temperature, body temperature, weather, carry load, stamina, sanity and other status. See [HUD](HUD.md). |
| Zone features | Center, upper | Resource nodes and other permanent or semi-permanent features of the zone. |
| Ground pool | Center, lower | Items on the ground in the zone ([Ground Pool](../Systems/GroundPool.md)). |
| Event log | Lower left | See [HUD: event log](HUD.md#event-log). |
| Inventory | Bottom | The survivor's inventory. Collapsible. |
| Nav bar | Right edge | Slim icon bar that expands on hover or click. Each icon opens a gameplay window: crafting, cooking, equipment, research, quests and others. If the player wants to do something, it is probably here. |

## HUD detail window

Clicking the HUD opens a tabbed window to explore each stat, macro and meter, buffs, debuffs, injuries, and other survivor and zone status information.

## Organization tools

The inventory, zone features and ground pool all provide the same organization tools, so the player can arrange things however they want:

- Sorting
- Filtering
- Player-created tabs
- Categories
- Tags
- Placeholders (reserved slots for items the player wants to keep a place for)
