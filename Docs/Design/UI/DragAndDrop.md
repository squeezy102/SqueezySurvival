# Drag and Drop

**Status:** Draft

How the player moves things around in grid panels: to rearrange them, and to combine one with another ([#44](https://github.com/squeezy102/SqueezySurvival/issues/44)).

## Principle

**Nothing happens until you let go, and the game tells you what will happen if you do** ([GDD: design principles](../GameDesignDocument.md#design-principles)). While a drag is held, the game previews the outcome; releasing carries it out; releasing anywhere without an outcome changes nothing.

## Dragging

- Press on any cell in a [grid panel](GridPanels.md) and move the mouse: a copy of the thing's icon follows the pointer, anywhere on the screen.
- **While dragging, nothing in the grid moves** ([#47](https://github.com/squeezy102/SqueezySurvival/issues/47)). The thing stays in its slot; as far as the game is concerned it has not moved. The only changes are the copy on the pointer, an outline on the slot it would go to, and a tooltip saying what letting go would do.
- **Letting go** puts the thing in the slot under the pointer, as the tooltip said. Letting go anywhere else changes nothing.
- A drag starts once the pointer has moved 6 px with the button held, so a plain click stays a click.
- Escape cancels the drag.
- The mouse wheel still scrolls a grid during a drag.

## Slots

A grid is a set of fixed slots, like a classic inventory. Slots are numbered in the grid's fill order (left to right then down, or top to bottom then right; [Grid Panels: scroll direction](GridPanels.md#scroll-direction)).

- Each kind of thing stays in its slot until the player moves it. **A drop only ever involves the dragged thing and the thing in the slot under the pointer**; nothing else moves.
- Slots can be left empty on purpose, to lay things out. Every slot is drawn, empty ones as faint squares, and there is always at least one empty slot after the last thing.
- A newly found kind takes the **first empty slot**, so gaps fill from the start without moving anything the player placed.
- Changing a grid's columns or rows keeps every thing in its numbered slot; the grid simply wraps differently.

## Drop zones

Each occupied cell has five hidden zones: a center and four edges. The player never sees their borders; they see what dropping there would do.

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
- **Edges:** the rest of the cell. All four edges do the same thing for now; they are kept separate for uses still to be designed.

| Let go over | What happens | Tooltip |
|---|---|---|
| An empty slot | The thing moves into it. | "Move. Place the stick here." |
| An edge of another thing | The two **swap** slots. | "Swap. The stick and the stone trade places." |
| The center of another thing | They **combine**, if they can (see below). | What the combination makes, or "Can't combine. The stick and the stone can't be combined." |
| The thing itself | Nothing. | "No change. It stays where it is." |
| Anywhere else, including another panel | Nothing. | - |

The target slot is outlined whenever letting go would change something.

## Combining

Dropping one thing on the center of another combines them when a recipe allows it: for example a stone on a stone to make a sharpened stone, or a sharpened stone on a crab to get crab meat. Recipes, their results and their time costs belong to crafting, which is not designed yet; until then nothing combines, and the preview says so.

## Accessibility

Every drag needs an alternative that needs neither a drag nor a held button ([Controls: accessibility](Controls.md#accessibility), [OQ-033](../OpenQuestions.md)).

## Implementation notes

- Drop zones, outcomes and their wording are decided in Core (`CellDropZoneClassifier`, `GridDropOutcomeResolver`) and unit tested; the presentation layer reports where the pointer is and carries out the outcome Core returns.
- Slots live in Core: `CountedCollection.MoveToSlot` moves or swaps.
- Which things combine is asked of an `IThingCombinationRules`; the wireframe's rules combine nothing.
