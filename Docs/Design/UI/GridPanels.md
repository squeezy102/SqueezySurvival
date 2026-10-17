# Grid Panels

**Status:** Draft

How the game screen's item and feature panels show their contents: as scrollable grids the player configures to suit how they like to play ([#41](https://github.com/squeezy102/SqueezySurvival/issues/41)).

## Panels

| Panel | Shows |
|---|---|
| Zone features | The zone's features ([Screen Layout: regions](ScreenLayout.md#regions)). |
| On the ground | The zone's ground pool ([Zones: ground pool](../Systems/Zones.md#ground-pool)). |

The inventory will use the same grid once it is built.

## The grid

- Contents are shown as a grid of square cells. Each kind of thing gets one cell, showing its icon, and its count in the bottom-right corner when there are 2 or more. Until icon art exists, icons are simple placeholder shapes drawn by the game.
- **The panel never resizes.** Its size comes from the screen layout ([Screen Layout: wireframe stage](ScreenLayout.md#wireframe-stage)). The grid fits the panel, and its contents scroll inside it.
- Cells are 8 px apart (at the 1920 x 1080 reference resolution), and the panel's header sits the same 8 px above the grid.
- **Cells never change size as things are added.** The scrollbar always has its place along the scroll direction's edge, 8 px from the cells. While everything fits, it is dimmed and disabled, and the mouse wheel does nothing; once the cells overflow, it is enabled ([#42](https://github.com/squeezy102/SqueezySurvival/issues/42)).

### Scroll direction

| Setting | Fixed | Cell size | Fill order | Scrolls |
|---|---|---|---|---|
| Vertical | Number of columns | Panel width divided by the columns | Left to right, then the next row down | Up and down |
| Horizontal | Number of rows | Panel height divided by the rows | Top to bottom, then the next column right | Left and right |

When the grid can scroll, the mouse wheel always scrolls along the panel's scroll direction: in a horizontal panel, rolling the wheel moves the grid left and right. By default, rolling the wheel up scrolls up (or left) and rolling it down scrolls down (or right). **Invert scroll wheel** reverses that. 

## Tooltips

Hovering over a cell shows a tooltip beside it ([#43](https://github.com/squeezy102/SqueezySurvival/issues/43)):

| Line | Example |
|---|---|
| Name | Stone |
| Description | A fist-sized stone, smooth from the surf. |
| Weight of one (items only) | Weight: 1 lb each |
| Weight of the stack (items only) | Stack of 3: 3 lb |

Weights are in pounds ([Inventory](../Systems/Inventory.md)). Descriptions and weights are placeholders until content and weights are designed ([OQ-056](../OpenQuestions.md)).

## Settings

Each panel has its own settings.

| Setting | Values | Default |
|---|---|---|
| Scroll direction | Vertical, Horizontal | Vertical |
| Columns (vertical) | 4 to 8 | 6 |
| Rows (horizontal) | 2 to 4 | 3 |
| Invert scroll wheel | On, Off | Off |

The maximums keep cells large enough to read in the panel's size; they may be adjusted after playtesting. Columns and rows are both kept, so switching the scroll direction back restores the earlier layout.

### Where they are changed

- **The gear in the panel's header** opens that panel's settings. Until icon art exists, the gear is a button labeled "Settings".
- **The Options screen** lists every panel's settings ([Options](Options.md)).

Both edit the same player options. A change applies at once, everywhere, and is saved automatically.

## Implementation notes

- The settings are one configuration object, `PlayerOptions`, shared by the game screen and the Options screen and held for the whole play session.
- The grid's geometry (cell size, cell positions, content size) and the wheel direction are calculated in Core (`GridPanelLayoutCalculator`, `GridPanelScrollWheelMapper`) and unit tested; the presentation layer only places cells where Core says.
- Placeholder icons are drawn with UI Toolkit's vector API ([Unity Manual: generate 2D visual content](https://docs.unity3d.com/Manual/UIE-generate-2d-visual-content.html)) by `PlaceholderIconPainter`, keyed by each definition's id. They need no image files.
- UI Toolkit's own tooltip only works in the Unity Editor ([Unity Manual: tooltip event](https://docs.unity3d.com/Manual/UIE-Tooltip-Events.html)), so the game draws its own (`GridTooltipView`); its content comes from Core (`ThingTooltipContentFactory`).
- Player options are saved as a text file in the player's data folder (Unity's [`Application.persistentDataPath`](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html)). They belong to the player, not to a run.
