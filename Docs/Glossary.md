# Glossary

Canonical terms. Code, UI text and documents use these words with these meanings.

| Term | Definition |
|---|---|
| Action | Anything the survivor does. Has a duration, costs, requirements, outcomes and risks. See [Actions](Design/Systems/Actions.md). |
| Biome | A category of environment (for example Beach) that defines which resources can be found. |
| Cost | What an action consumes: time, and optionally stats, materials or tool durability. |
| Food category | A grouping of foods such as Meat, Fish, Fruit, Vegetables or Dairy. Has its own tolerance meter. |
| Food tolerance | A per-food meter (100% to 0%) of how willing the survivor is to eat that food. Falls with each serving, recovers over time; low tolerance reduces what the food restores and causes penalties. See [Food](Design/Systems/Food.md). |
| Decay | Deterioration of items over time, at a per-item rate, slowed by storage. Inorganic items decay very slowly. See [Decay](Design/Systems/Decay.md). |
| Event log | The on-screen running record of game events in plain language. See [HUD](Design/UI/HUD.md#event-log). |
| Exertion risk | The chance that a strenuous action injures the arms or legs; zero above 50% stamina and rising as stamina falls. See [Exertion](Design/Systems/Exertion.md). |
| Exploration progress | A zone's explored percentage. Rises with each explore; milestones trigger significant discoveries. See [Exploration](Design/Systems/Exploration.md). |
| Find pool | The weighted set of loose items an explore can draw in a zone. |
| Ground pool | The items lying on the ground in a zone. Unlimited in size; not part of the inventory. See [Ground Pool](Design/Systems/GroundPool.md). |
| Hazard | A danger met while exploring, such as a jellyfish or sea urchin. |
| Inventory | Items the survivor is carrying: the personal inventory plus any carriers. See [Inventory](Design/Systems/Inventory.md). |
| Known | Encountered by the survivor. Only known things are ever shown in the UI. |
| Light level | Bright, Dim or Dark. Derived from the day phase and light sources; gates and penalizes actions. See [Actions](Design/Systems/Actions.md#light-level). |
| Recipe | A definition of inputs, tools and time required to produce an item. |
| Resource node | A fixed zone feature harvested with actions, such as a banana tree or spring. Renewable actions let it replenish; destructive actions consume it. See [Resource Nodes](Design/Systems/ResourceNodes.md). |
| Research | The passive, time-based process that turns a discovered recipe into a craftable one. |
| Macro | Informal name for a survival stat. |
| Survival stat | A core condition of the survivor shown as a bar where higher is always better, such as Food or Water. See [Survival Stats](Design/Systems/SurvivalStats.md). |
| Carrier | Equipment that adds its own weight-limited inventory, such as a basket, backpack or cart. See [Inventory](Design/Systems/Inventory.md). |
| Day | 1,440 TU (24 hours of game time). |
| Day phase | One of six named parts of the day (Dawn, Morning, Midday, Afternoon, Dusk, Night) that other systems use to vary behavior. See [Time](Design/Systems/Time.md). |
| Time unit (TU) | The smallest unit of game time: 1 minute. See [Time](Design/Systems/Time.md). |
| Zone | A discrete location belonging to a biome, connected to other zones. |
| Zone connection | A discovered route from one zone into another. |
