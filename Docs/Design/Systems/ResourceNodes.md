# Resource Nodes

**Status:** Draft

A resource node is a fixed feature of a zone, found through [exploration](Exploration.md), that the survivor harvests with actions. Examples: palm trees, banana trees, bushes, springs, beehives, tide pools.

## Harvest actions

A node offers one or more harvest actions. Each is one of two kinds (resolves OQ-024):

| Kind | Effect on the node | Examples |
|---|---|---|
| Renewable | Takes from the node's current yield. The node stays and replenishes. | Climb a banana tree for bananas. Pick berries from a bush. Collect water from a spring. |
| Destructive | Consumes the node. It is gone from the zone. | Chop down a tree for logs. Dig up a bush for its roots. Harvest a beehive. |

The same node can offer both: a bush can be picked repeatedly, or dug up once.

## Replenishment

Renewable yields return through a trigger, defined per node.

| Trigger | Example |
|---|---|
| Time | A banana tree regrows bananas over several days. |
| Weather | A puddle or spring refills only after rain. |
| Tide | A tide pool restocks with the tide. |

## Throwing stones

A survivor with stones in their inventory and a free hand can throw them at nodes out of reach. As in [combat](Combat.md#throwing-stones), each thrown stone has a decent chance of landing in the zone's ground pool to be picked up again. Some throws carry risks:

| Target | Result | Risk |
|---|---|---|
| High-hanging fruit | Knocks it down | - |
| Beehive | Knocks it down | The bees may become angry |
| Bird's nest | Knocks it down | The eggs will probably crack |

## Access conditions

A harvest action can have requirements beyond tools and light ([Actions](Actions.md)), including conditions on the zone itself.

Example: a beehive can only be harvested safely at night after a campfire has been built in the zone, because the smoke calms the bees.

## Consumed nodes

A consumed node does not return. Exploration can later reveal a new node of the same type, which is a different node (a different beehive), not the old one restored.
