# Drag and Drop

**Status:** Draft

How the player moves things around in [inventory grids](InventoryGrids.md): to rearrange them, and to combine one with another ([#44](https://github.com/squeezy102/SqueezySurvival/issues/44)).

## Principle

**Nothing happens until you let go, and the game tells you what will happen if you do** ([GDD: design principles](../GameDesignDocument.md#design-principles)). While a drag is held, the game previews the outcome; releasing carries it out; releasing anywhere without an outcome changes nothing.

## Dragging

- Press on any cell in an [inventory grid](InventoryGrids.md) and move the mouse: a copy of the thing's icon follows the pointer, anywhere on the screen.
- **While dragging, nothing in the grid moves** ([#47](https://github.com/squeezy102/SqueezySurvival/issues/47)). The thing stays in its slot; as far as the game is concerned it has not moved. The only changes are the copy on the pointer, an outline on the slot it would go to, and a tooltip saying what letting go would do.
- **Letting go** puts the thing in the slot under the pointer, as the tooltip said. Letting go anywhere else changes nothing.
- A drag starts once the pointer has moved 6 px with the button held, so a plain click stays a click.
- Escape cancels the drag.

## Drop zones

Grids work like Diablo's inventory ([#49](https://github.com/squeezy102/SqueezySurvival/issues/49)). Over an occupied cell, a drag distinguishes the center from the rest of the cell. The player never sees the border; they see what dropping there would do.

- **Center:** the middle 30% of the cell in each direction.
- **The rest of the cell:** everything outside the center.

| Let go over | What happens | Tooltip |
|---|---|---|
| An empty slot | The thing moves into it; its old slot is left empty. | "Move. Put the lizard here." |
| Another thing, outside its center | The two **swap** slots. Nothing else moves. | "Swap. The lizard and the paper trade places." |
| The center of another thing | They **combine**, if they can (see below). | What the combination makes, or "Can't combine. The lizard and the paper can't be combined." |
| Itself | Nothing. | "No change. It stays where it is." |
| Anywhere else, including another panel | Nothing. | - |

While dragging, an outline marks the slot the thing will go to or combine with. Nothing in the grid moves until release.

### Top and bottom edges: bulk moves (planned)

The top and bottom edges of a cell are reserved for click-and-hold bulk moves between the inventory and the ground:

- **Holding the top edge of a stack in the inventory** moves it to the ground one item at a time, faster the longer it is held.
- **Holding the bottom edge of a stack on the ground** moves it into the inventory the same way.

Built with the inventory ([#6](https://github.com/squeezy102/SqueezySurvival/issues/6)).

## Combining

Dropping one thing on the center of another combines them when a recipe allows it: for example a stone on a stone to make a sharpened stone, or a sharpened stone on a crab to get crab meat. Recipes, their results and their time costs belong to crafting, which is not designed yet; until then nothing combines, and the preview says so.

## Accessibility

Every drag needs an alternative that needs neither a drag nor a held button ([Controls: accessibility](Controls.md#accessibility), [OQ-033](../OpenQuestions.md)).

## Implementation notes

- Drop zones, outcomes and their wording are decided in Core (`CellDropZoneClassifier`, `GridDropOutcomeResolver`) and unit tested; the presentation layer reports where the pointer is and carries out the outcome Core returns.
- Slots live in Core: `CountedCollection.MoveToSlot` moves a thing into an empty slot or swaps it with the thing there.
- Which things combine is asked of an `IThingCombinationRules`; the wireframe's rules combine nothing.
