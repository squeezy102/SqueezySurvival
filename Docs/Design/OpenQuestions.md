# Open Questions

Design questions still waiting for an answer. The full history of every question, open and resolved, is in the [Question Log](QuestionLog.md).

**Process**

1. **New question:** take the next ID, add it here and to the Question Log as Open.
2. **Partly answered:** resolve it with what was settled, and open a new question for the rest. Never edit a question's wording.
3. **Deferred:** a question that needs numbers best found by testing moves to the deferred table below and is answered in a dedicated balancing pass.
4. **Resolved:** record the answer in the owning document, remove the row here, and in the Question Log set the status to Resolved with a one-line answer and link. Both files change in the same commit.

| ID | Area | Question | Owner doc |
|---|---|---|---|
| OQ-071 | Controls | Click to pick up and place: what puts a picked-up thing back (Escape, right click, clicking outside any grid), how combining works without a drag (the same one-second hover over a thing?), and can a picked-up thing be carried to another panel or a container? | UI/Controls |
| OQ-072 | Controls | Modifier pairing: are modifiers paired by how much moves or by where it goes? Proposal (by how much, destination implied): left click picks up and places; **Shift + left click** sends the whole stack to the other side (inventory and ground, or a container and where it stands); **Ctrl + left click** sends half; **Alt + left click** sends one; right click opens the thing's actions. | UI/Controls |
| OQ-073 | Controls | Remapping: squeezy102 expects mouse buttons and modifier keys not to be remappable, but the Game Accessibility Guidelines Basic tier, our baseline, asks that controls can be remapped. Remap mouse and modifiers too, or record an exception to the baseline? | UI/Controls |
| OQ-074 | Runs | Run perks: which traits, items and challenges can survival and challenge points buy, at what prices, and what penalty offsets each easier choice? (The rest of OQ-058.) | Systems/Runs |
| OQ-075 | Actions | Pacing: should long actions be capped in real time? At 15 game minutes per second, a 1-hour Rest takes 4 seconds and an 8-hour sleep 32. Proposal: scale at that rate up to a cap of about 3 seconds. | Systems/Actions |
| OQ-076 | Research | Is research done per single thing (a stone axe, a campfire) or per field (stone tools, firemaking)? | Systems/Research |
| OQ-079 | Equipment | Durability: how forgiving is it, what wears things out (use, taking and dealing damage, rain and storms), how is it shown (a bar on the item, uses left in the tooltip, a broken-equipment icon), and what happens to broken equipment (keeps its slot, gives no benefit until repaired, can break mid-fight)? squeezy102 wants a dedicated session. | Systems/Equipment |
| OQ-080 | Content | Chase items and quests: which hard-won items are worth working toward (a turtle shell shield, a legendary pirate cutlass), and which quests exist (a talking seagull, a pirate ghost, a plane overhead after day 50 prompting a smoke signal)? Backlog, for a dedicated session. | GameDesignDocument |
| OQ-081 | Runs | Endings: should a run have story goals, such as bosses, key places, a final boss and a final scene (escape, or embracing life in the wild)? An ending never ends the run: the player is told and keeps playing. | Systems/Runs |
| OQ-082 | Content | Messages in bottles from contributors and backers: who can submit, what review keeps them appropriate, and how are names and consent handled? | Content/BeachFinds |
| OQ-083 | Crafting | Crafting stations (a forge with fuel and ore): are they worked from the crafting window or from their own window? | Systems/Crafting |
| OQ-084 | Inventory | Haulers and carrying help: when are the sled and the cart available, and how much of the load in a carrier (a hand basket, a backpack) or a hauler counts toward what the survivor carries? Today's rule says none of a carrier's load counts for movement; squeezy102 describes the hand basket as a slight help. (The rest of OQ-078.) | Systems/Inventory |

## Deferred to the balancing pass

Answered together in a dedicated mathematical balancing pass.

| ID | Area | Question | Owner doc |
|---|---|---|---|
| OQ-004 | Stats | Stat ranges, decay rates per time unit, thresholds and their effects. | Systems/SurvivalStats |
| OQ-036 | Encounters | Encounter chance per minute of action, and per zone, phase and weather. | Systems/Encounters |
| OQ-056 | Inventory | Weight balancing: the personal weight limit, carrier capacities, item weights, and whether load can exceed 100%. Weight is a delicate balancing lever affecting carrying, exploration, exertion, food logistics and crafting. **Deferred to a dedicated weight balancing session.** | Systems/Inventory |
| OQ-057 | Combat | Damage values for weapons and animals, animal health, and which injuries each animal inflicts. | Systems/Combat |
| OQ-059 | Runs | Easy and hard mode: how much lower (easy) or higher (hard) are penalties, decay and stat depletion? | Systems/Runs |
| OQ-061 | Weather | Wildfire tuning: the chance an unattended fire starts one, how long the zone burns, how much is turned to charcoal, destroyed or damaged and how fast, the exact escape window, and how fast harm escalates for a survivor who stays past it. | Systems/Weather |
| OQ-062 | Combat | Tracking: how do the animal's injury and the survivor's Tracking skill combine into the chance of finding it? | Systems/Combat |
| OQ-063 | Weather | Weather balancing: the strength of each weather effect (for example how fast sunburn builds), and the exact per-season transition weights that produce the seasonal tendencies. | Systems/Weather |
| OQ-065 | Combat | Getting away for good: the chance an escaping animal that survives is gone for good, and how much it grows with each escape. | Systems/Combat |
| OQ-069 | Injuries | Health recovery and knock-out tuning: regeneration per hour for each activity (Wait, Rest, Nap, Sleep; open or sheltered; bed), and how overkill sets the knock-out length. Proposal: the survivor wakes once health regenerates from minus the overkill back up to 1. | Systems/Injuries |
