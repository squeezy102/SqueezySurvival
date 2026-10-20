# Controls

**Status:** Draft

The game is played almost entirely with the mouse. The keyboard provides modifiers and hotkeys.

## No character movement

There is no walking or character movement. Travelling between zones, exploring and every other action is done by clicking.

## Mouse

| Input | Available |
|---|---|
| Left click, right click | Yes |
| Click and drag | Yes |
| Shift, Ctrl, Alt, and their combinations, with either button | Yes |

Dragging in inventory grids is defined in [Drag and Drop](DragAndDrop.md).

### Principles (from OQ-032)

- **Left click** does the quick action; **right click** opens the actions available for what was clicked. Right-clicking a tree lists Climb, Chop and so on.
- **Modifier keys pair up consistently.** Similar actions always use the same modifier, either by kind of action (one modifier moves a whole stack, another half, another one item) or by destination (one modifier always moves to the inventory, another always to the ground). The game never asks for unrelated combinations for related actions. Which pairing is used: [OQ-072](../OpenQuestions.md).
- **Moving things** covers: move all, move half (split a stack), move one, to and from containers, and drop on the ground.
- **Deleting is never a click.** The far right of the inventory has a **trash slot**: an item dropped there stays until something else is dropped on it, which replaces it and destroys it for good, as in Terraria and Stardew Valley. An **Empty trash** button under it destroys what is there at once.

### Click to pick up and place

Every drag has a click alternative (resolves OQ-033): **clicking a thing picks it up onto the cursor, and clicking again puts it down**, with the same outcomes as dragging and letting go ([Drag and Drop](DragAndDrop.md)). This replaces clicking to select a thing ([#56](https://github.com/squeezy102/SqueezySurvival/issues/56)). The details: [OQ-071](../OpenQuestions.md).

## Keyboard hotkeys (defaults)

| Key | Does |
|---|---|
| Space | Explore. The Explore button says so. |
| M | Map |
| C | Crafting |
| S | Skills |

- **Letter, number and symbol keys are only ever hotkeys** for opening windows and common actions. Moving and interacting are done with the mouse and modifier keys.
- **Every hotkey is shown where it is used:** the Crafting button's tooltip says it opens with C.
- The full list grows as windows are designed.

## Accessibility

Baseline: [Game Accessibility Guidelines](https://gameaccessibilityguidelines.com/full-list/), Basic tier ([GDD](../GameDesignDocument.md#pillars)).

- **All hotkeys can be remapped.** Whether mouse buttons and modifier keys can be remapped too, as the Basic tier asks ("Allow controls to be remapped / reconfigured"): [OQ-073](../OpenQuestions.md).
- Every drag has a click alternative, so nothing needs a held button ([Click to pick up and place](#click-to-pick-up-and-place), Intermediate tier).
