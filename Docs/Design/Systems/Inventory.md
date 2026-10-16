# Inventory

**Status:** Draft

What the survivor carries. Weights are in pounds (lb).

## Personal inventory

The survivor has a static personal weight limit. It does not grow with progression; carrying more requires carriers. The limit's value: [OQ-029](../OpenQuestions.md).

### Load

Load is the personal inventory's weight as a percentage of the limit.

| Load | State | Effect |
|---|---|---|
| Below 75% | Normal | None. |
| 75-99% | Overburdened | Stamina costs of actions increase, compounding as load rises. |
| 100% | Immobile | Local actions remain available; any action that moves the survivor (exploring, travelling to another zone) is unavailable until load drops below 100%. |

### Overburdened stamina formula (proposed, not approved)

`stamina cost multiplier = 1.04 ^ (load% - 75)` for load from 75% to 99%.

| Load | Multiplier |
|---|---|
| 75% | 1.00x |
| 80% | 1.22x |
| 90% | 1.80x |
| 99% | 2.56x |

## Carriers

Carriers are equipment that add their own weight-limited inventory.

| Carrier | Availability |
|---|---|
| Basket | Early |
| Belt | Early |
| Backpack | Mid |
| Wagon, cart, sleigh | Late |

- Each carrier's inventory has its own weight limit.
- Weight in a carrier does not count against the survivor's personal limit for the purposes of movement.
- Worn carriers (basket, belt, backpack) occupy an equipment slot. Example: a backpack might hold 5 more sticks, at the cost of the slot it uses.
- How vehicles (wagon, cart, sleigh) are moved and used is specified with zone travel.

Equipment slots are specified in Equipment (planned).
