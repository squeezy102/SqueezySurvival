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

## Moving, swapping and combining

Grids work like Diablo's inventory ([#49](https://github.com/squeezy102/SqueezySurvival/issues/49)). A quick drag and drop rearranges; holding the thing over another for a moment turns the drop into an attempt to combine.

| Let go over | What happens | Tooltip |
|---|---|---|
| An empty slot | The thing moves into it; its old slot is left empty. | "Move. Put the lizard here." |
| Another thing | The two **swap** slots. Nothing else moves. | "Swap. The lizard and the paper trade places." |
| Another thing, after holding over it for **1 second** | They **combine**, if they can (see below). | What the combination makes, or "Can't combine. The lizard and the paper can't be combined." |
| Itself | Nothing. | "No change. It stays where it is." |
| Itself, after holding over it for **1 second** | Two of the stack **combine with each other**, if they can: two stones make a sharpened stone. A single thing can't. | What the combination makes, "Can't combine. The stone and the stone can't be combined.", or "Can't combine. There is only one stone. It takes two to combine one with itself." |
| Another panel | See [Between panels](#between-panels). | |
| Anywhere else | Nothing. | - |

- The hold starts when the pointer enters an occupied cell, the thing's own included, and resets when it leaves; moving within the cell does not reset it. The tooltip changes from Swap or No change to the combine preview when the second is up. The time is a placeholder, to be tuned in play.
- **A combine that can't happen changes nothing:** on release the thing goes back to its slot, and a message by that slot says so ("The lizard and the paper can't be combined.") for 2 seconds. Nothing is logged.
- While dragging, an outline marks the slot the thing will go to or combine with. Nothing in the grid moves until release.

## Between panels

Things move between the **ground** and the **inventory** ([#86](https://github.com/squeezy102/SqueezySurvival/issues/86)). Zone features are not things to carry and stay in their own grid; the hands are filled through [Equipment](../Systems/Equipment.md), later.

- **A plain drag moves the whole stack.** Holding a modifier while letting go moves part of it: **Ctrl moves half** (rounded up: half of 7 is 4) and **Alt moves one**. Shift also moves the whole stack. Players can remap which key does which ([Controls: accessibility](Controls.md#accessibility)).
- **A modified click sends things straight to the other panel**, ground to inventory or inventory to ground, with no drag: **Shift + click** sends the whole stack, **Ctrl + click** half, **Alt + click** one (resolves OQ-085). Where modified clicks send things once containers exist: [OQ-086](../OpenQuestions.md).
- **Things join their own kind (auto-stack).** When the other panel already holds that kind, what moves joins that stack, wherever it is let go. Otherwise it takes the empty slot it was let go over, or, over another thing, the first empty slot; nothing is swapped between panels. Auto-stacking can be turned off in a later option ([#106](https://github.com/squeezy102/SqueezySurvival/issues/106)).
- **When the other panel has no room** (every slot taken by other kinds), nothing moves and the tooltip says so: "No room. There's no free slot in your inventory." A drop onto a full ground does not ask what to discard; that question is only for finds.
- **There is no weight cap** ([Inventory: weight](../Systems/Inventory.md#weight)): anything can be picked up, and the load shows what it costs.
- **Moving between panels takes a few seconds**, and the tooltip says how long before letting go ([Inventory: moving things](../Systems/Inventory.md#moving-things)).

| Let go over (other panel) | Tooltip |
|---|---|
| An empty slot, kind not there yet | "Pick up. Put 5 stones in your inventory. Takes 10 sec." / "Put down. Put 5 stones on the ground. Takes 10 sec." |
| Anywhere in the panel, kind already there | "Stack. Add 5 stones to the 12 in your inventory. Takes 10 sec." / "Stack. Add 5 stones to the 12 on the ground. Takes 10 sec." |
| Another thing, kind not there yet | As for an empty slot; the outline marks the first empty slot. |
| No room | "No room. There's no free slot in your inventory." |

### Top and bottom edges: bulk moves (planned)

The top and bottom edges of a cell are reserved for click-and-hold bulk moves between the inventory and the ground:

- **Holding the top edge of a stack in the inventory** moves it to the ground one item at a time, faster the longer it is held.
- **Holding the bottom edge of a stack on the ground** moves it into the inventory the same way.

Designed and built in [#103](https://github.com/squeezy102/SqueezySurvival/issues/103).

## Combining

Holding one thing over another and letting go combines them when a recipe allows it: for example a stone on a stone to make a sharpened stone, or a sharpened stone on a crab to get crab meat. Recipes, their results and their time costs belong to crafting, which is not designed yet; until then nothing combines, and the preview says so.

## Accessibility

Every drag needs an alternative that needs neither a drag nor a held button ([Controls: accessibility](Controls.md#accessibility), [OQ-033](../OpenQuestions.md)).

## Implementation notes

- The hold, outcomes and their wording are decided in Core (`CombineHoldTimer`, `GridDropOutcomeResolver`) and unit tested; the presentation layer reports where the pointer is and how much time has passed, and carries out the outcome Core returns.
- Slots live in Core: `CountedCollection.MoveToSlot` moves a thing into an empty slot or swaps it with the thing there.
- Moves between panels are decided in Core: `RunSession.PreviewItemMove` and `MoveItems` (with `CountedCollection.SlotToReceive`, `Receive` and `Take`), worded by `ItemMoveTextFormatter`; which modifier means which amount is `MoveModifierBindings`. The presentation layer (`ItemPanelMoveCoordinator`) only finds the grid under the pointer and reads the keyboard.
- Which things combine is asked of an `IThingCombinationRules`; the wireframe's rules combine nothing.
