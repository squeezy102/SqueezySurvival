# Inventory Grids

**Status:** Draft

How the game screen shows items and zone features: fixed grids of slots, like Diablo's inventory ([#49](https://github.com/squeezy102/SqueezySurvival/issues/49)). A grid never scrolls and has no layout settings. When it is full, it is full.

## The grids

| Grid | Panel | Size | Holds |
|---|---|---|---|
| Ground | On the ground | 16 x 4 (64 slots) | The zone's [ground pool](../Systems/Zones.md#ground-pool) |
| Zone features | Zone features | 16 x 4 (64 slots) | The zone's features |
| Inventory | Inventory | 8 slots in one row | What the survivor carries ([Inventory](../Systems/Inventory.md#slots)) |
| Hands | Inventory, far left, set apart | 2 slots | The tool hand and offhand ([Equipment](../Systems/Equipment.md)) |

Sizes are placeholders, to be tuned in play.

- Each slot holds one kind of thing, with its count in the bottom-right corner when there are 2 or more. A grid of 64 slots holds 64 different kinds; a kind's count has no limit.
- Every slot is drawn, empty ones as faint squares. The cells are square and as large as the panel allows; the grid is centered in its panel. **The panel never resizes.**
- Cells are 8 px apart (at the 1920 x 1080 reference resolution), and the panel's header sits the same 8 px above the grid.
- Things stay in their slots until the player moves them ([Drag and Drop](DragAndDrop.md)); slots may be left empty on purpose. A newly arriving kind takes the first empty slot.
- Icons are placeholder pixel art drawn by the game until real art exists ([Art Direction](../ArtDirection.md#icons)).

## When the ground is full

Exploring can find a new kind when the ground's 64 slots are full. The game stops and asks, rather than losing anything silently:

> There's no room on the ground for the conch.
>
> **Leave the conch behind** - it will be gone for good.
> **Choose something to discard** - pick something on the ground to throw away; it will be gone for good, and the conch takes its place.

Choosing something to discard highlights the ground's slots; hovering one says what will happen ("Discard the stone (3) to make room for the conch. It will be gone for good."), and clicking it does it. Escape returns to the question. Either choice is logged. Each new kind that does not fit is asked about in turn.

Zone features beyond their grid's 64 slots are not revealed.

## Sorting

The ground and inventory panels have a **Sort** button in their header, labeled with the panel's sort order ("Sort: By type"). It packs everything into the first slots, closing gaps, in that order.

- The default order is **by type**: grouped by [item category](../Systems/Inventory.md#item-categories) in the order listed there, then by name.
- The arrow beside Sort opens the other orders: **alphabetical**, **by quantity** (most first) and **by weight** (heaviest stack first). Choosing one sorts at once and becomes that panel's order for the Sort button.

## Tooltips

Hovering over a cell shows a tooltip beside it ([#43](https://github.com/squeezy102/SqueezySurvival/issues/43)):

| Line | Example |
|---|---|
| Name | Stone |
| Category (items) | Stone and minerals |
| Description | A fist-sized stone, smooth from the surf. |
| Weight of one (items) | Weight: 1 lb each |
| Weight of the stack (items) | Stack of 3: 3 lb |

Weights are in pounds ([Inventory](../Systems/Inventory.md)). Descriptions and weights are placeholders until content and weights are designed ([OQ-056](../OpenQuestions.md)).

## Implementation notes

- A grid's slots, capacity, overflow and sorting live in Core (`CountedCollection`, `ThingSortComparer`); its geometry comes from `FixedGridLayoutCalculator`. The presentation layer only places cells where Core says (`InventoryGridView`), and the sort menu is UI Toolkit's `GenericDropdownMenu` ([Unity Scripting API](https://docs.unity3d.com/ScriptReference/UIElements.GenericDropdownMenu.html)).
- UI Toolkit's own tooltip only works in the Unity Editor ([Unity Manual: tooltip event](https://docs.unity3d.com/Manual/UIE-Tooltip-Events.html)), so the game draws its own (`GridTooltipView`); its content comes from Core (`ThingTooltipContentFactory`).
- Placeholder icons are 16 x 16 pixel art drawn by the game from `PlaceholderIconCatalog`, keyed by each definition's id ([Art Direction](../ArtDirection.md#icons)). They need no image files.
