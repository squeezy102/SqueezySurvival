# Ground Pool

**Status:** Draft

Each zone has a ground pool: the items lying on the ground there. Items enter it from [exploration](Exploration.md), harvesting [resource nodes](ResourceNodes.md), and anything the survivor puts down. Items in a zone's ground pool are not in the survivor's inventory and stay in that zone.

## Capacity

The ground pool has no capacity limit (resolves OQ-028). Items leave it only by:

| Mechanism | Affects |
|---|---|
| The survivor picking them up or using them | Anything |
| [Decay](Decay.md) | Everything; organic items within days, inorganic items only after a very long time |
| Extreme weather events (floods, storms, landslides and similar) | Anything |

Extreme weather events are specified in Weather (planned, [OQ-009](../OpenQuestions.md)).
