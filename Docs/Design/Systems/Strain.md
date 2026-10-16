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

- A visible body value from 0 to 100, shown as a body status warning before it causes harm ([Pillar 4: clarity](../GameDesignDocument.md#pillars)).
- Resting, or doing non-strenuous actions, gradually reduces it.
- How accumulated strain leads to injury: [OQ-030](../OpenQuestions.md).

Injuries are specified in their own system (planned).
