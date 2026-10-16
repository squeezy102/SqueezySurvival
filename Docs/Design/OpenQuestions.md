# Open Questions

Design questions still waiting for an answer. The full history of every question, open and resolved, is in the [Question Log](QuestionLog.md).

**Process**

1. **New question:** take the next ID, add it here and to the Question Log as Open.
2. **Resolved:** record the answer in the owning document, remove the row here, and in the Question Log set the status to Resolved with a one-line answer and link. Both files change in the same commit.

| ID | Area | Question | Owner doc |
|---|---|---|---|
| OQ-004 | Stats | Stat ranges, decay rates per time unit, thresholds and their effects. | Systems/SurvivalStats |
| OQ-008 | Zones | How large is the island at MVP: how many beach zones, and how are they connected? | Systems/Zones |
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
| OQ-047 | Runs | What perks, boosts and challenge unlocks can survival and challenge points buy, and at what prices? (Earn rates: 1 survival point per day, 1 challenge point per 30 days.) Deferred to a dedicated session. | Systems/Runs |
| OQ-049 | Runs | Easy and hard mode: how much lower (easy) or higher (hard) are penalties, decay and stat depletion? (Points settled: easy earns 1/5 survival points and no challenge points; hard earns double.) | Systems/Runs |
| OQ-052 | Weather | What does each weather state do to stats, actions, temperature and the environment, and how does weather change over time (per zone, per phase, per season)? | Systems/Weather |
| OQ-053 | Weather | Which seasons exist, how long are they, and what changes between them besides wildfire risk? | Systems/Weather |
| OQ-054 | Weather | Wildfire tuning: the chance an unattended fire starts one, how long the zone stays closed, how much is turned to charcoal, destroyed or damaged, the exact escape window (15-30 minutes), and how fast harm escalates for a survivor who stays past it. | Systems/Weather |
