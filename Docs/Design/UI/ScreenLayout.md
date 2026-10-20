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
| HUD | Top left corner | Survival stats, outside temperature, body temperature, weather, game clock, carry load, stamina, sanity and other status (clock placement resolves OQ-035). See [HUD](HUD.md). |
| Zone features | Center, upper | Resource nodes and other permanent or semi-permanent features of the zone. |
| Ground pool | Center, lower | Items on the ground in the zone ([Zones: ground pool](../Systems/Zones.md#ground-pool)). |
| Event log | Lower left | See [HUD: event log](HUD.md#event-log). |
| Inventory | Bottom | The survivor's inventory. Collapsible. |
| Nav bar | Right edge | Slim bar of icon buttons. Each opens a gameplay window: crafting, cooking, equipment, research, quests and others. If the player wants to do something, it is probably here. Hovering a button shows a tooltip with its name and what it is for. Options sits apart at the bottom. The inventory has no button: it is always on screen ([#54](https://github.com/squeezy102/SqueezySurvival/issues/54)). |

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

## Wireframe stage

The first playable wireframe of this screen. Regions whose systems are not built yet are shown as labeled placeholders.

Spacing is uniform: every panel is inset 16 px from the screen edge and separated from its neighbors by 16 px (at the 1920 x 1080 reference resolution). Panels stretch to fill the space between them, so there are no empty gaps. Panel sizes come from the screen, never from their content: a panel's content scrolls inside it rather than resizing the layout.

### Text containment

**Text never leaves its container** ([#67](https://github.com/squeezy102/SqueezySurvival/issues/67)). Every label and button wraps its text inside its own width and grows taller to fit it; a container either fits its text or grows with it. Text running past a container's edge and into the scene is a bug, never a style choice.

- Wrapping is the default for every screen, set once in the runtime theme, not opted into element by element.
- An element that holds text never has a fixed height, only a minimum height. Fixed sizes are for regions, such as panels and grids, whose contents are laid out to fit them.
- Every screen is checked with its longest text, such as an unavailable option's reason in place of its label.

| Region | Built | Placeholder |
|---|---|---|
| Scene | Placeholder beach art. | - |
| HUD | The day and night dial ([Time: display](../Systems/Time.md#display)) beside the game clock: the day, the time and the day phase, one per row, for example `Day 1`, `6:00 AM` and `Dawn` ([#58](https://github.com/squeezy102/SqueezySurvival/issues/58)) ([Time: display](../Systems/Time.md#display)). Wait (15 min) and Rest (1 hr) buttons, with their durations shown ([Time: passing time deliberately](../Systems/Time.md#passing-time-deliberately)). | Survival stats, temperature and weather. |
| Zone bar | Current zone name (Beach), its exploration progress ("Explored 4%"), and Explore with its duration ([Exploration: wireframe stage](../Systems/Exploration.md#wireframe-stage)). | No connected zones are known at the start. |
| Zone features, ground pool | Two panels, "Zone features" above "On the ground", each a fixed 16 x 4 grid of what exploring has found ([Inventory Grids](InventoryGrids.md)). The ground has Sort with its order menu, and asks what to discard when it is full. | Filtering. |
| Event log | Every action is logged with its time, starting with waking on the beach. | - |
| Inventory | The 2 hand slots, set apart on the far left, and 8 inventory slots, with Sort ([Inventory Grids](InventoryGrids.md#the-grids)). | Empty: nothing can be picked up yet. |
| Nav bar | Icon buttons, top to bottom: Map (a map), Crafting (a hammer and anvil), Skills (a book), Research (a vial and an atom), Equipment (a crossed sword and pickaxe) and Cooking (a steaming pot); then, apart at the bottom, Options (a gear), which opens the [Options](Options.md) screen. Each shows a tooltip on hover. The bar is 104 px wide; the panels to its left take the space. | The gameplay windows open a notice that they aren't built yet, and their tooltips say so. |

Escape opens a pause menu holding **Options** ([Options](Options.md)) and **Save and exit**, the one way out of a run ([Runs: saving](../Systems/Runs.md#saving)). Escape again closes it. Saving does not exist yet, so for now Save and exit returns to the main menu without keeping the run, and the menu says so.

The game screen only opens from a confirmed New Game ([Startup Flow: starting the run](StartupFlow.md#starting-the-run)). Opened any other way (for example, pressing Play with the game scene open in the editor), it returns to the main menu.
