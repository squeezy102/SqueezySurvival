# Drag and Drop

**Status:** Draft

How the player moves things around in grid panels: to rearrange them, and to combine one with another ([#44](https://github.com/squeezy102/SqueezySurvival/issues/44)).

## Principle

**Nothing happens until you let go, and the game tells you what will happen if you do** ([GDD: design principles](../GameDesignDocument.md#design-principles)). While a drag is held, the game previews the outcome; releasing carries it out; releasing anywhere without an outcome changes nothing.

## Dragging

- Press on any cell in a [grid panel](GridPanels.md) and move the mouse: the thing's icon follows the pointer, anywhere on the screen. The cell it came from is dimmed.
- A drag starts once the pointer has moved 6 px with the button held, so a plain click stays a click.
- Escape cancels the drag. Letting go where nothing would happen also cancels it: the thing returns to where it came from.
- The mouse wheel still scrolls a grid during a drag.

## Drop zones

Each cell has five hidden zones. The player never sees their borders; they see what dropping there would do.

```
+-------------------+
|        Top        |
|      +-----+      |
| Left | Ctr | Right|
|      +-----+      |
|      Bottom       |
+-------------------+
```

- **Center:** the middle half of the cell in each direction.
- **Edges:** the rest of the cell, each part belonging to the nearest edge.

| Drop on | Outcome |
|---|---|
| Left or top edge of a cell | Move the dragged thing to just **before** that cell. |
| Right or bottom edge of a cell | Move it to just **after** that cell. |
| Center of a cell | **Combine** the two, if they can be combined (see below). |
| Empty space in the same grid, after the last cell | Move it to the **end**. |
| Anywhere else, including another panel | Nothing; it returns. |

A grid's contents are one ordered list, filled left to right then down (vertical) or top to bottom then right (horizontal) ([Grid Panels: scroll direction](GridPanels.md#scroll-direction)). "Before" and "after" are places in that list, which is why all four edges mean one of the two. The insertion marker is drawn on the side of the cell where the thing will actually land, whichever edge is hovered. A move that would leave the thing where it already is shows "No change".

The order is part of the game's state, not just the screen: it stays as the player left it, new finds of a kind already there add to its cell, and new kinds join at the end.

## Preview

While over a drop zone, the game shows:

| Outcome | Marker | Tooltip beside the target cell |
|---|---|---|
| Move before or after | A bar on the side of the target cell where the thing will land | "Move: place the stick before the stone." |
| Move to the end | - | "Move: place the stick at the end." |
| No change | - | "No change." |
| Combine | The target cell is highlighted | What the combination makes, and what it costs |
| Cannot combine | - | "The stick and the stone can't be combined." |

## Combining

Dropping one thing on the center of another combines them when a recipe allows it: for example a stone on a stone to make a sharpened stone, or a sharpened stone on a crab to get crab meat. Recipes, their results and their time costs belong to crafting, which is not designed yet; until then nothing combines, and the preview says so.

## Accessibility

Every drag needs an alternative that needs neither a drag nor a held button ([Controls: accessibility](Controls.md#accessibility), [OQ-033](../OpenQuestions.md)).

## Implementation notes

- Drop zones, outcomes and their wording are decided in Core (`CellDropZoneClassifier`, `GridDropOutcomeResolver`) and unit tested; the presentation layer reports where the pointer is and carries out the outcome Core returns.
- Reordering is `CountedCollection.Move`, so the order lives in the zone's state.
- Which things combine is asked of an `IThingCombinationRules`; the wireframe's rules combine nothing.
