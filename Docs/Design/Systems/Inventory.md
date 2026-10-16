# Inventory

**Status:** Draft

What the survivor carries. Weights are in pounds (lb).

## Personal inventory

The survivor has a static personal weight limit. It does not grow with progression; carrying more requires carriers. All weight values (the personal limit, carrier capacities, item weights) are a delicate balancing lever and are set in a dedicated weight balancing session ([OQ-056](../OpenQuestions.md)).

### Load

Load is the personal inventory's weight as a percentage of the limit.

| Load | State | Effect |
|---|---|---|
| Below 75% | Normal | None. |
| 75-99% | Overburdened | Stamina costs of actions increase, compounding as load rises. |
| 100% | Immobile | Local actions remain available; any action that moves the survivor (exploring, travelling to another zone) is unavailable until load drops below 100%. |

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

## Carriers

Carriers are equipment that add their own weight-limited inventory.

| Carrier | Availability |
|---|---|
| Hand basket | Early; carried in the offhand, slightly increases carrying capacity |
| Belt | Early |
| Backpack | Mid |
| Wagon, cart, sleigh | Late |

- Each carrier's inventory has its own weight limit.
- Weight in a carrier does not count against the survivor's personal limit for the purposes of movement.
- Worn and held carriers occupy an equipment slot: the hand basket uses the offhand, the belt the belt slot, the backpack the back slot. Example: a backpack might hold 5 more sticks, at the cost of the slot it uses.
- How vehicles (wagon, cart, sleigh) are moved and used is specified with zone travel.

Equipment slots are specified in [Equipment](Equipment.md).
