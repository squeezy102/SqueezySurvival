# Open Questions

Design questions still waiting for an answer. The full history of every question, open and resolved, is in the [Question Log](QuestionLog.md).

**Process**

1. **New question:** take the next ID, add it here and to the Question Log as Open.
2. **Resolved:** record the answer in the owning document, remove the row here, and in the Question Log set the status to Resolved with a one-line answer and link. Both files change in the same commit.

| ID | Area | Question | Owner doc |
|---|---|---|---|
| OQ-004 | Stats | Stat ranges, decay rates per time unit, thresholds and their effects. | Systems/SurvivalStats |
| OQ-008 | Zones | How large is the island at MVP: how many beach zones, and how are they connected? | Systems/Zones |
| OQ-009 | Weather | Which weather states exist and how do they affect stats and actions? | Systems/Weather (planned) |
| OQ-025 | Exploration | How does exploring work at Dawn, Dusk and Night (light level penalties, hazard and injury chances)? | Systems/Exploration |
| OQ-029 | Inventory | Weight balancing: the personal weight limit, carrier capacities, item weights, and whether load can exceed 100%. Weight is a delicate balancing lever affecting carrying, exploration, exertion, food logistics and crafting. **Deferred to a dedicated weight balancing session.** | Systems/Inventory |
| OQ-032 | Controls | What does each mouse button and modifier combination do (for example Shift + left click to move a whole stack, Ctrl + left click to move one)? Defined per screen as screens are designed. | UI/Controls |
| OQ-033 | Controls | Alternatives to drag for moving items. Ideas to explore: click to pick up and click to place; double-click to move; click and hold to move many. Note: the Game Accessibility Guidelines' Intermediate tier asks to avoid requiring both drags and held buttons, so each method needs an alternative that does not. | UI/Controls |
| OQ-036 | Encounters | Encounter chance per minute of action, and per zone, phase and weather. | Systems/Encounters |
| OQ-037 | Combat | Damage values for weapons and animals, animal health, and which injuries each animal inflicts. | Systems/Combat |
| OQ-038 | Combat | How does each pair of actions resolve (Fight vs Defend, Flee vs Fight, etc.)? Proposal in [Combat](Systems/Combat.md#turn-resolution). | Systems/Combat |
| OQ-040 | Equipment | Which hand is the tool hand? Stated as left hand = tool, right hand = offhand. Proposal: a dominant-hand setting (right by default) decides which side is the tool hand. | Systems/Equipment |
| OQ-043 | Equipment | Saved loadouts: can the player save gear sets and switch between them with one click outside combat, at a small time cost? | Systems/Equipment |
| OQ-044 | Combat | Thrown stones: sling ammunition only (the earliest ranged weapon is a crafted sling with a pouch), or a separate weak one-handed throw action usable with anything in hand? | Systems/Combat |
| OQ-046 | Research | Are topics with unmet requirements hidden, or shown locked with their requirements listed? | Systems/Research |
| OQ-047 | Runs | How many survival and challenge points does a run award, and what perks, boosts and challenge unlocks can they buy? Deferred to a dedicated session. | Systems/Runs |
| OQ-049 | Runs | Easy mode: how much lower are penalties, decay and stat depletion, and what share of points does an easy run earn? Proposal: 25% of normal points. | Systems/Runs |
| OQ-050 | Runs | Casual saves: when all 5 save instances are in use, which is replaced? Proposal: a new autosave replaces the oldest autosave; a manual save asks which save to overwrite. | Systems/Runs |
