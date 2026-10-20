# Inventory

**Status:** Draft

What the survivor carries. Weights are in pounds (lb).

## Slots

The survivor's inventory has **8 slots**, plus the tool hand and offhand ([Equipment](Equipment.md)). Each slot holds one kind of item, any number of it. Slots limit how many kinds the survivor carries at once; weight limits how much ([#49](https://github.com/squeezy102/SqueezySurvival/issues/49)). Shown as the inventory's [grid](../UI/InventoryGrids.md#the-grids).

## Item categories

Every item belongs to one category. Categories group items in tooltips and sorting ("by type" sorts in this order):

| Category | For example |
|---|---|
| Wood | Sticks, branches, logs, shavings, driftwood, bamboo |
| Plant matter | Algae, seaweed, fronds, leaves, dry leaves, grass, bark |
| Stone and minerals | Stones, flint, pumice, coral, clay, sand |
| Shells | Seashells, conches, sand dollars |
| Fruit | Coconuts, bananas, berries |
| Vegetables | Roots, tubers, edible leaves |
| Meat and fish | Raw and cooked meat, fish, crab meat |
| Animal parts | Feathers, bones, hide, crab shells, starfish, sponges |
| Animals | Live animals, such as a caught crab |
| Carcasses | Whole dead animals, to be butchered |
| Salvage | Washed-up man-made things: glass, rope, floats |
| Crafting materials | Made intermediates: cordage, sharpened stones, planks |
| Tools and weapons | Knives, axes, spears, slings |

## Weight

The survivor has a static personal weight limit of **100 lb**, a placeholder until the weight balancing pass ([OQ-087](../OpenQuestions.md)). It does not grow with progression; carrying more requires carriers.

**There is no cap on what the survivor carries** (resolves part of OQ-056). Anything can be picked up, however heavy: 500,000 stones if the player wants. Weight only ever limits moving and travelling, through load below.

### Load

Load is the weight of everything the survivor carries, the inventory and both hands, as a percentage of the limit. It can go past 100%.

| Load | State | Effect |
|---|---|---|
| Below 75% | Normal | None. |
| 75-99% | Overburdened | Stamina costs of actions increase, compounding as load rises. |
| 100% and over | Immobile | Local actions remain available; any action that moves the survivor (exploring, travelling to another zone) is unavailable until load drops below 100%. |

### Overburdened penalties (proposed, not approved)

Intent: just over 75% is effectively free; near 100%, moving can make the survivor pass out or sprain something.

**Stamina cost** of every action:

`multiplier = 1 + 7 x x^3`, where `x = (load% - 75) / 25`

| Load | Multiplier | Feel |
|---|---|---|
| 76% | 1.0004x | Unnoticeable |
| 80% | 1.004x | Unnoticeable |
| 85% | 1.45x | Noticeable |
| 90% | 2.5x | Heavy |
| 95% | 4.6x | Punishing |
| 99% | 7.2x | A single move can exhaust the survivor |

**Injury.** Carrying a heavy load is a high-risk strenuous action for the legs. Overburdened movement drains stamina quickly, and low stamina brings injury risk. See [Exertion](Exertion.md).

**Passing out:** a survivor whose Stamina reaches 0 collapses. This is a general Stamina rule ([Survival Stats](SurvivalStats.md)), which heavy loads make likely.

## Moving things

Things move between the ground and the inventory by dragging, by modified clicks ([Drag and Drop: between panels](../UI/DragAndDrop.md#between-panels)) and, later, by the stack's edges ([#103](https://github.com/squeezy102/SqueezySurvival/issues/103)).

- **Rearranging within one panel is free.**
- **Moving between panels costs seconds by weight**, so it is never free but never feels like a penalty: lifting a pound of rocks takes a moment. Placeholder: **2 seconds per lb moved**, rounded up to a whole second, at least 1 second. The clock counts them like any other time ([Time: rules](Time.md#rules)). The rate is set in the weight balancing pass ([OQ-087](../OpenQuestions.md)).
- Every move between panels is logged: "You picked up 5 stones." / "You put 2 sticks on the ground."

## Carriers

Carriers are **inventory expansion objects**: equipment that adds its own weight-limited inventory to what the survivor carries. They are not [placeable storage](Containers.md), which stands in a zone and gives no carrying help (resolves OQ-078).

| Carrier | Availability |
|---|---|
| Hand basket | Early. Held in either hand, so two can be carried. A slight carrying help and extra capacity, for foraging and gathering trips. |
| Belt | Early |
| Backpack | Mid |
| Wagon, cart, sleigh | Late. When sleds and carts arrive, and how much of their load counts: [OQ-084](../OpenQuestions.md) |

- Each carrier's inventory has its own weight limit.
- Weight in a carrier does not count against the survivor's personal limit for the purposes of movement.
- Worn and held carriers occupy an equipment slot: a hand basket uses a hand, the belt the belt slot, the backpack the back slot. Example: a backpack might hold 5 more sticks, at the cost of the slot it uses.
- How vehicles (wagon, cart, sleigh) are moved and used is specified with zone travel.

Equipment slots are specified in [Equipment](Equipment.md).
