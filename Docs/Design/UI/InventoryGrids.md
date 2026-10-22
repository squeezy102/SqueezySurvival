# Inventory Grids

**Status:** Draft

How the game screen shows items and zone features: fixed grids of slots, like Diablo's inventory ([#49](https://github.com/squeezy102/SqueezySurvival/issues/49)). A grid never scrolls and has no layout settings. When it is full, it is full.

## The grids

| Grid | Panel | Size | Holds |
|---|---|---|---|
| Ground | On the ground | 12 x 3 (36 slots) | The zone's [ground pool](../Systems/Zones.md#ground-pool) |
| Zone features | Zone features | 12 x 3 (36 slots) | The zone's features |
| Inventory | Inventory | 8 slots in one row | What the survivor carries ([Inventory](../Systems/Inventory.md#slots)). The panel's header shows the load as text ("Load: 34 / 100 lb (34%), Normal") until the load gauge replaces it ([#104](https://github.com/squeezy102/SqueezySurvival/issues/104)). |
| Hands | Inventory, far left, set apart | 2 slots | The left hand, then the right hand; the dominant one is the tool hand ([Equipment: handedness](../Systems/Equipment.md#handedness)) |

Sizes are placeholders, to be tuned in play. The ground and features grids were 16 x 4; squeezy102 cut them to 12 x 3 to try (2026-10-19): the survivor doesn't need that many slots there, and fewer slots make bigger, more readable icons.

The hands must never be mistaken for inventory slots ([#57](https://github.com/squeezy102/SqueezySurvival/issues/57)): a bold gold rule with a wide gap on each side separates them, and an empty hand slot shows a faint hand, the left hand's on the left, seen from behind with fingers up.

- Each slot holds one kind of thing, with its count in the bottom-right corner when there are 2 or more. A grid of 36 slots holds 36 different kinds; a kind's count has no limit. The player can split a kind across several slots ([Drag and Drop: splitting and merging](DragAndDrop.md#splitting-and-merging)); things that arrive join the kind's first stack.
- Every slot is drawn, empty ones as faint squares. The cells are square and as large as the panel allows; the grid is centered in its panel. **The panel never resizes.**
- Cells are 8 px apart (at the 1920 x 1080 reference resolution), and the panel's header sits the same 8 px above the grid.
- Things stay in their slots until the player moves them ([Drag and Drop](DragAndDrop.md)); slots may be left empty on purpose. A newly arriving kind takes the first empty slot.
- Icons are placeholder pixel art drawn by the game until real art exists ([Art Direction](../ArtDirection.md#icons)).

## When the ground is full

Exploring can find a new kind when the ground's slots are all full. The game stops and asks, rather than losing anything silently:

> There's no room on the ground for the conch.
>
> **Leave the conch behind** - it will be gone for good.
> **Choose something to discard** - pick something on the ground to throw away; it will be gone for good, and the conch takes its place.

Choosing something to discard highlights the ground's slots; hovering one says what will happen ("Discard the stone (3) to make room for the conch. It will be gone for good."), and clicking it does it. Escape returns to the question. Either choice is logged. Each new kind that does not fit is asked about in turn.

Zone features beyond their grid's slots are not revealed.

## Sorting

The ground and inventory panels have a **Sort** button in their header, labeled with the panel's sort order ("Sort: By type"). It packs everything into the first slots, closing gaps, in that order, and joins split stacks of the same kind back together.

- The default order is **by type**: grouped by [item category](../Systems/Inventory.md#item-categories) in the order listed there, then by name.
- The arrow beside Sort opens the other orders: **alphabetical**, **by quantity** (most first) and **by weight** (heaviest stack first). Choosing one sorts at once and becomes that panel's order for the Sort button.

## Selecting

Clicking a thing selected it and kept its tooltip open ([#56](https://github.com/squeezy102/SqueezySurvival/issues/56)). This is being replaced: clicking picks the thing up onto the cursor instead ([Controls: click to pick up and place](Controls.md#click-to-pick-up-and-place)).

## Tooltips

Hovering over a cell shows a tooltip beside it ([#43](https://github.com/squeezy102/SqueezySurvival/issues/43)):

| Line | Example |
|---|---|
| Icon | The thing's icon beside its name, one whole-pixel step larger than in its slot ([#55](https://github.com/squeezy102/SqueezySurvival/issues/55)) |
| Name | Stone |
| Category (items) | Stone and minerals |
| Description | A fist-sized stone, smooth from the surf. |
| Weight of one (items) | Weight: 1 lb each |
| Weight of the stack (items) | Stack of 3: 3 lb |

Weights are in pounds ([Inventory](../Systems/Inventory.md)). Descriptions and weights are placeholders until content and weights are designed ([OQ-087](../OpenQuestions.md)).

## Implementation notes

- A grid's slots, capacity, overflow and sorting live in Core (`CountedCollection`, `ThingSortComparer`); its geometry comes from `FixedGridLayoutCalculator`. The presentation layer only places cells where Core says (`InventoryGridView`), and the sort menu is UI Toolkit's `GenericDropdownMenu` ([Unity Scripting API](https://docs.unity3d.com/ScriptReference/UIElements.GenericDropdownMenu.html)).
- **Each grid makes one cell per slot when it is built and keeps it** (`InventoryGridCell`, [#114](https://github.com/squeezy102/SqueezySurvival/issues/114)). A change to the contents updates only the cells whose contents differ, in place, so hover, selection and drags elsewhere in the grid are undisturbed. Unity advises reusing elements rather than creating and discarding them, since creating elements and adding them to the hierarchy is slow ([Unity Manual: best practices for managing elements](https://docs.unity3d.com/2022.1/Documentation/Manual/UIE-best-practices-for-managing-elements.html)). Core names the slots that changed ([Architecture: commands, state and change events](../../Engineering/Architecture.md#commands-state-and-change-events), [#121](https://github.com/squeezy102/SqueezySurvival/issues/121)), so the view touches only those.
- UI Toolkit's own tooltip only works in the Unity Editor ([Unity Manual: tooltip event](https://docs.unity3d.com/Manual/UIE-Tooltip-Events.html)), so the game draws its own (`GridTooltipView`); its content comes from Core (`ThingTooltipContentFactory`).
- Placeholder icons are 16 x 16 pixel art drawn by the game from `PlaceholderIconCatalog`, keyed by each definition's id ([Art Direction](../ArtDirection.md#icons)). They need no image files.
