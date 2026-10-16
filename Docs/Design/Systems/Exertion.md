# Exertion

**Status:** Draft

How physical effort leads to injury. Stamina is the only effort meter; there is no separate strain meter.

## Responsibilities

| Piece | Its one job |
|---|---|
| Stamina | The effort budget. Physical actions spend it; rest refills it ([Survival Stats](SurvivalStats.md)). |
| Exertion risk | Below 50% stamina, strenuous actions can cause injury. The lower the stamina, the higher the chance. |
| Injury | The consequence, applied to the body region the action used. |
| Passing out | The backstop. At 0 stamina the survivor collapses, losing time; the collapse itself may cause injury. |
| Rest | Refills stamina, which removes the risk. |

## Body regions

Injuries apply to one of two regions: **arms** or **legs**. No other regions are tracked.

| Activity | Region |
|---|---|
| Chopping, crafting, knapping, building, foraging, skinning | Arms |
| Exploring, travelling, carrying heavy loads | Legs |
| Swimming, climbing | Both |

- An injured arm affects arm actions; an injured leg affects movement actions.
- Shown as a small body silhouette whose arms and legs change state, using shading or pattern as well as color.

## Risk tiers

Every strenuous action has a risk tier, which sets its base injury chance.

| Tier | Example actions | Base injury chance (proposed) |
|---|---|---|
| Minimal | Foraging, knapping stone, skinning a small animal | 0.5% |
| Medium | Chopping wood, building, crafting normal-sized items | 2% |
| High | Swimming or climbing with low skill, carrying heavy loads | 5% |

## Exertion risk formula (proposed, not approved)

`injury chance per action = base chance x stamina factor`

| Stamina | Stamina factor |
|---|---|
| 50% and above | 0 (no risk) |
| 25% | 1x |
| 0% | 2x |

Linear between those points: `stamina factor = (50 - stamina%) / 25` below 50%.

**Exception:** high-risk actions that are dangerous in themselves (low-skill climbing or swimming) keep a small inherent injury chance at any stamina. Heavy loads raise stamina costs ([Inventory: load](Inventory.md#load)), so they drive stamina down and risk up.

## Warnings

Falling stamina is reported in the [event log](../UI/HUD.md#event-log) before risk begins and as it grows, for example:

| Stamina | Example log entry |
|---|---|
| 60% | "Your arms are getting tired." |
| 45% | "Your arms ache." |
| 20% | "Your arms are trembling." |

## Example: a morning of chopping wood

| Time | Stamina | What happens |
|---|---|---|
| 7:00 AM | 100% | Starts chopping. No risk. |
| 9:00 AM | 60% | Still safe. "Your arms are getting tired." |
| 10:00 AM | 45% | Risk begins: each chop has a small chance of an arm injury. |
| 11:00 AM | 20% | Risk is high. The player chooses: rest, or keep going and gamble. |
| Rest an hour | 70% | Safe again. |
| Keep going | 0% | Passes out, loses time, and may be injured by the collapse. |

## Repetition

Repetitive work is never penalized. Variety is encouraged through Contentment rewards instead ([Survival Stats: contentment](SurvivalStats.md#contentment)).
