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

- Each kind of thing stays in its slot until the player moves it.
- Slots can be left empty on purpose, to lay things out. Every slot is drawn, empty ones as faint squares, and there is always at least one empty slot after the last thing.
- A newly found kind takes the **first empty slot**, so gaps fill from the start without moving anything the player placed.
- Changing a grid's columns or rows keeps every thing in its numbered slot; the grid simply wraps differently.
- **Shifting:** when a thing is put into an occupied slot, that thing and the ones after it move one slot later, up to the first empty slot, which absorbs the shift. Things only ever move later, never earlier, and nothing past that gap moves. The slot the dragged thing left counts as empty.

Example ([#47](https://github.com/squeezy102/SqueezySurvival/issues/47)): rock, paper, scissors, lizard and spock fill slots 1-5. Dragging the lizard onto the left side of the paper shows "Put the lizard before the paper." On release, the lizard takes slot 2; paper and scissors shift to slots 3 and 4, filling the lizard's old slot; spock stays in slot 5.

## Drop zones

Over an occupied cell, a drag distinguishes three zones. The player never sees their borders; they see what dropping there would do.

```
+---------+---------+
|      +-----+      |
| Left | Ctr | Right|
|      +-----+      |
+---------+---------+
```

- **Center:** the middle 30% of the cell in each direction.
- **Left and right:** the rest of the cell's left and right halves.

| Let go over | What happens | Tooltip |
|---|---|---|
| An empty slot | The thing moves into it. | "Move. Put the lizard here." |
| The left side of another thing | The thing goes **before** it: into the empty slot just before it if there is one, otherwise into its slot, shifting it later. | "Move. Put the lizard before the paper." |
| The right side of another thing | The thing goes **after** it: into the slot after it, shifting whatever is there later. | "Move. Put the lizard after the paper." |
| The center of another thing | They **combine**, if they can (see below). | What the combination makes, or "Can't combine. The lizard and the paper can't be combined." |
| Itself, or a place that would leave it where it is | Nothing. | "No change. It stays where it is." |
| Anywhere else, including another panel | Nothing. | - |

"Before" and "after" follow the slot order in both scroll directions. While dragging, a bar in the gap on that side of the cell marks a before or after; an outline marks a move into an empty slot or a combination. Nothing in the grid moves until release.

### Top and bottom edges: bulk moves (planned)

The top and bottom edges of a cell are not used by dragging. They are for click-and-hold bulk moves between the inventory and the ground:

- **Holding the top edge of a stack in the inventory** moves it to the ground one item at a time, faster the longer it is held.
- **Holding the bottom edge of a stack on the ground** moves it into the inventory the same way.

Built with the inventory ([#6](https://github.com/squeezy102/SqueezySurvival/issues/6)).

## Combining

Dropping one thing on the center of another combines them when a recipe allows it: for example a stone on a stone to make a sharpened stone, or a sharpened stone on a crab to get crab meat. Recipes, their results and their time costs belong to crafting, which is not designed yet; until then nothing combines, and the preview says so.

## Accessibility

Every drag needs an alternative that needs neither a drag nor a held button ([Controls: accessibility](Controls.md#accessibility), [OQ-033](../OpenQuestions.md)).

## Implementation notes

- Drop zones, outcomes and their wording are decided in Core (`CellDropZoneClassifier`, `GridDropOutcomeResolver`) and unit tested; the presentation layer reports where the pointer is and carries out the outcome Core returns.
- Slots live in Core: `CountedCollection.MoveToSlot` moves a thing and shifts later things up to the first gap.
- Which things combine is asked of an `IThingCombinationRules`; the wireframe's rules combine nothing.
