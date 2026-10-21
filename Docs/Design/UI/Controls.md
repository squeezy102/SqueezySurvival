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
- **Modifier keys pair up by how much moves** (resolves OQ-072): one modifier moves all of a stack, one moves half, one moves one. The same modifier means the same amount everywhere. Defaults (resolves OQ-085): **Shift moves all, Ctrl moves half, Alt moves one**; a modified click sends things to the other panel ([Drag and Drop: between panels](DragAndDrop.md#between-panels)). An open container takes modified clicks before the inventory (resolves OQ-086); where clicks in the container go: [OQ-088](../OpenQuestions.md).
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

## Escape

Escape always backs out of the most immediate thing, so the player can press it repeatedly to get back to the game. On the game screen it does the **first** of these that applies:

1. During a drag: cancel the drag ([Drag and Drop](DragAndDrop.md)).
2. With something selected: clear the selection.
3. While choosing what to discard for a find with no room: return to the question ([Inventory Grids: when the ground is full](InventoryGrids.md#when-the-ground-is-full)).
4. While that question is showing: nothing. It must be answered.
5. With a "not built yet" notice open: close it.
6. With [Options](Options.md) open: close it and return to where it was opened.
7. Otherwise: open the pause menu, or close it if it is open ([Screen Layout](ScreenLayout.md)).

Other documents say what Escape does in their own context; this list is the one home for the order.

## Accessibility

Baseline: [Game Accessibility Guidelines](https://gameaccessibilityguidelines.com/full-list/), Basic tier ([GDD](../GameDesignDocument.md#pillars)).

- **All hotkeys can be remapped** ("Allow controls to be remapped / reconfigured", Basic).
- **Modifiers are remapped by role, not by key** (resolves OQ-073): the player chooses which of Shift, Ctrl and Alt does **Move all**, **Move half** and **Move one**. Each key does one role, so the scheme always stays consistent.
- **Mouse buttons are not remapped in the game.** Players who want the buttons swapped use Windows' own setting (Settings > Bluetooth & devices > Mouse > Primary mouse button), which applies to every program.
- Every drag has a click alternative, so nothing needs a held button ([Click to pick up and place](#click-to-pick-up-and-place), Intermediate tier).
