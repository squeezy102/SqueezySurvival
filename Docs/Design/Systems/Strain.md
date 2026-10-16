# Strain

**Status:** Draft

Physical strain from strenuous actions, and the injuries it leads to. Injuries can happen instantly, but mostly result from repeated negligence.

## Strain risk

Every strenuous action carries a strain risk tier, a multiplier on the magnitude of risk.

| Tier | Example actions |
|---|---|
| Minimal | Foraging, knapping stone, skinning a small animal |
| Medium | Chopping wood, building, crafting normal-sized items |
| High | Swimming with low skill, climbing with low skill, carrying heavy loads |

Skill and load modify the tier: low skill and heavy loads raise risk ([Inventory: load](Inventory.md#load)).

## Body regions

Strain and injuries are tracked separately for two regions: **arms** and **legs**. No other regions are tracked.

| Activity | Region |
|---|---|
| Chopping, crafting, knapping, building, foraging, skinning | Arms |
| Exploring, travelling | Legs |
| Swimming, climbing | Both |
| Carrying heavy loads | Legs |

- Each region has its own strain value; an action's roll applies to the region it uses.
- Switching to an activity that uses the other region lets strained muscles recover while work continues.
- Injuries are localized: an injured leg affects movement actions, an injured arm affects arm actions.
- Shown as a small body silhouette whose arms and legs change state, using shading or pattern as well as color.

## Outcome roll

Each strenuous action rolls one of three outcomes. The risk tier sets the weights.

| Outcome | Effect |
|---|---|
| Nothing | No effect. |
| Accumulate strain | The survivor's strain value rises. |
| Instant injury | The survivor is injured immediately (for example a sprain). |

### Weights (proposed, not approved)

| Tier | Nothing | Accumulate strain | Instant injury |
|---|---|---|---|
| Minimal | 97% | 2.9% | 0.1% |
| Medium | 90% | 9.5% | 0.5% |
| High | 75% | 23% | 2% |

## Strain value

- One value per body region, from 0 to 100, shown on the body silhouette before it causes harm ([Pillar 4: clarity](../GameDesignDocument.md#pillars)).
- Resting, or doing non-strenuous actions, gradually reduces it.
- **Strain multiplies the injury roll.** The higher the survivor's strain, the greater the instant-injury weight of every strenuous action (resolves OQ-030). The multiplier's curve is set during balancing.
- Rising strain is reported in the [event log](../UI/HUD.md#event-log) before it causes harm, for example "You felt strain in your muscles", then "Your muscles ache".

Injuries are specified in their own system (planned).
