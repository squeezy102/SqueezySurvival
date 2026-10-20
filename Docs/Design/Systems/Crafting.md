# Crafting

**Status:** Concept - proposals from squeezy102's notes (2026-10-18), not approved

Crafting makes new things from what the survivor has. The survivor can only craft recipes they have discovered by trying, or learned through [research](Research.md) ([You only know what you know](../GameDesignDocument.md#pillars)).

## Two ways to craft

| Way | How | Where the result goes |
|---|---|---|
| **By hand** | Some recipes need no window: hold one thing over another and let go ([Drag and Drop: combining](../UI/DragAndDrop.md#combining)). Two stones make a sharpened stone. | Where the two things came from. If they came from different places, the inventory first, then the ground. |
| **In the crafting window** | Other recipes are made in the window. | See below. |

Some recipes need a **crafting station** in the zone: copper bars need a refining forge, which has its own inventory for ore and fuel, a way to light it, and a way to take the bars out. Whether stations are worked from the crafting window or their own: [OQ-083](../OpenQuestions.md).

## The crafting window

- Opened from the nav bar's Crafting button (or C). It covers the zone features panel, at the same size; the ground grid stays usable.
- Recipes are **sortable, searchable and grouped into categories**. A player who knows what they want only ever argues with time and materials, never with the menu.
- Choosing a recipe shows slots for its ingredients. Drag them in; once all are there, Craft is available.
- The result goes to the inventory if there is room, else the ground, else it waits in the window's output slot, and no more crafting happens until it is taken. A setting (gear in the window's header) keeps results in the window instead; on by default.
- The header settings also choose the default sort.

### Works in progress

A **work in progress (WIP)** holds a recipe's ingredients as they are gathered: the player adds what they have now and the rest later. Once complete, Craft is available. **Cancel** deletes the WIP and returns its ingredients to the ground. Up to **5 WIPs** at a time.

## Costs

- **Ingredients should make sense, simplified for play.** A felling axe takes a long stick, several cords, a stone and a flint slab. Details such as splitting the stick or wrapping a grip are implied by the recipe, its picture and its description, never asked for.
- **Time should make sense, up to a point.** Most recipes take a couple of hours at most. Large builds (huts, sheds) can take days, but in stages (foundation, floor, walls, frame, roof), each one a finished step; the building is usable only when complete.
- **Conditions matter and compound.** Darkness, fog, wet materials, injury and sickness raise the chance of mistakes, wasted materials and injury, and lower the survivor's mood. A healthy survivor working dry, sheltered, well lit, with good tools and the right station does better, spends less energy, and may feel better for it.

## First recipes

To be decided. The tutorial's first crafts are a starting point ([Tutorial](../UI/Tutorial.md)).
