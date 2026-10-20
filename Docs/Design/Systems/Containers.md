# Containers

**Status:** Concept - proposals from squeezy102's notes (2026-10-19), not approved

Containers hold items in their own inventory and stand in a zone, on the ground or as a zone feature. Haulers (a sled, a cart) are containers built for moving heavy loads between zones.

## Containers

| Rule | Proposal |
|---|---|
| Where they stand | In a zone's ground grid, or as a zone feature. Some fit only indoors. |
| Their inventory | Each has its own grid and its own capacity, both varying by container. It sorts like any other grid ([Inventory Grids: sorting](../UI/InventoryGrids.md#sorting)). |
| Weight | A container weighs its own weight plus its contents. |
| Carrying | Only in **both hands**: the survivor puts down anything held to pick one up, and it counts fully toward what the survivor carries. A container can never go in an inventory slot, so it can't be used to carry more than 8 kinds of things. Some are too heavy to carry even empty. |
| No nesting | A container can't hold another container, even a smaller one. |
| How many | As many as there are free slots where they stand. |
| Names | Each can be renamed. Its tooltip shows its name and how full it is, as a percentage of its capacity. |
| Empty all | A button tips everything out onto the ground, after an "Are you sure?" with a Don't show this again checkbox. |
| First container | The **woven palm basket**, an early craft. Later: clay vessels, chests, cabinets. |

### Affinities

Like Path of Exile's stash tabs, a container can be set to take certain categories of items. When the survivor puts an item of that category down where the container stands, it goes into the container instead of onto the ground, filling matching containers left to right. Several containers can share an affinity. Dragging an item out of a container to somewhere specific always goes where the player put it.

## Haulers

A **sled** (early) and a **hand cart** (mid game; needs good woodworking and refined materials) carry large loads of few kinds of things between zones.

- They stand only on the ground or in the survivor's two hands; never in an inventory slot or as a zone feature.
- The survivor pulls one by holding it in both hands; put down, it stays where it is.
- They load like any container.
- **Only part of the load counts** toward what the survivor carries, so a full hauler is the whole job: the survivor carries little else, perhaps one tool. Backpacks, sacks and baskets help a little.
- **Few slots, large capacity:** a sled has about 2 slots, a cart about 4. As a rough comparison, a sled carries 3 logs back to camp without strain; a cart carries 8.

## Conflicts with approved design

- [Inventory: carriers](Inventory.md#carriers) puts a **hand basket in the offhand** as a carrier whose weight doesn't count toward movement, and lists wagons, carts and sleighs as late game. These notes make a woven basket a **two-handed container that counts fully**, with a sled early and a cart mid game. Which stands: [OQ-078](../OpenQuestions.md).
- "Don't show this again" checkboxes need somewhere to store the choice: the settings system ([#46](https://github.com/squeezy102/SqueezySurvival/issues/46)).
