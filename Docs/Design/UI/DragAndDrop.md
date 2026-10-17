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

## Slots

A grid is a set of fixed slots, like a classic inventory ([#47](https://github.com/squeezy102/SqueezySurvival/issues/47)). Slots are numbered in the grid's fill order (left to right then down, or top to bottom then right; [Grid Panels: scroll direction](GridPanels.md#scroll-direction)).

- Each kind of thing stays in its slot until the player moves it. **A move only ever involves the dragged thing and at most one other**, which trades places with it; nothing else shifts.
- Slots can be left empty on purpose, to lay things out. Every slot is drawn, empty ones as faint squares, and there is always at least one empty slot after the last thing.
- A newly found kind takes the **first empty slot**, so gaps fill from the start without moving anything the player placed.
- Changing a grid's columns or rows keeps every thing in its numbered slot; the grid simply wraps differently.

## Drop zones

Each occupied cell has five hidden zones. The player never sees their borders; they see what dropping there would do.

```
+-------------------+
|        Top        |
|      +-----+      |
| Left | Ctr | Right|
|      +-----+      |
|      Bottom       |
+-------------------+
```

- **Center:** the middle 30% of the cell in each direction.
- **Edges:** the rest of the cell, each part belonging to the nearest edge.

| Drop on | Outcome |
|---|---|
| An empty slot | Move the dragged thing into it. |
| An edge of a cell | The slot on that side of the cell (left, right, above or below): move into it if it is empty, or **swap** with the thing in it. |
| An edge whose slot is the dragged thing's own | Nothing changes. |
| An edge at the grid's side, with no slot beyond it | Nothing changes: "There is no space on that side." |
| The center of a cell | **Combine** the two, if they can be combined (see below). |
| Anywhere else, including another panel | Nothing; it returns. |

## Preview

While over a drop zone, the game shows the result before it happens:

| Outcome | The grid | Tooltip |
|---|---|---|
| Move | The dragged thing appears, dimmed, in the slot it will move to; its old slot shows empty. | "Move. Place the stick right of the stone." or "Place the stick here." |
| Swap | The two things appear in each other's slots. | "Swap. The stick and the shell trade places." |
| No change | Everything stays in place. | "No change." and why |
| Combine | The target cell is outlined. | Beside the target: what the combination makes, and what it costs. |
| Cannot combine | Everything stays in place. | "Can't combine. The stick and the stone can't be combined." |

## Combining

Dropping one thing on the center of another combines them when a recipe allows it: for example a stone on a stone to make a sharpened stone, or a sharpened stone on a crab to get crab meat. Recipes, their results and their time costs belong to crafting, which is not designed yet; until then nothing combines, and the preview says so.

## Accessibility

Every drag needs an alternative that needs neither a drag nor a held button ([Controls: accessibility](Controls.md#accessibility), [OQ-033](../OpenQuestions.md)).

## Implementation notes

- Drop zones, outcomes and their wording are decided in Core (`CellDropZoneClassifier`, `GridDropOutcomeResolver`) and unit tested; the presentation layer reports where the pointer is and carries out the outcome Core returns.
- Hit-testing uses the real slots, not the previewed ones, so a preview never moves the target out from under the pointer.
- Slots live in Core: `CountedCollection.MoveToSlot` moves or swaps, and `GridSlotNeighborhood` finds the slot beside another.
- Which things combine is asked of an `IThingCombinationRules`; the wireframe's rules combine nothing.
