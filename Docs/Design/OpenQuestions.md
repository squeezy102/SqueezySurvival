# Open Questions

Design questions still waiting for an answer. The full history of every question, open and resolved, is in the [Question Log](QuestionLog.md).

**Process**

1. **New question:** take the next ID, add it here and to the Question Log as Open.
2. **Partly answered:** resolve it with what was settled, and open a new question for the rest. Never edit a question's wording.
3. **Deferred:** a question that needs numbers best found by testing moves to the deferred table below and is answered in a dedicated balancing pass.
4. **Resolved:** record the answer in the owning document, remove the row here, and in the Question Log set the status to Resolved with a one-line answer and link. Both files change in the same commit.

| ID | Area | Question | Owner doc |
|---|---|---|---|
| OQ-025 | Exploration | How does exploring work at Dawn, Dusk and Night (light level penalties, hazard and injury chances)? | Systems/Exploration |
| OQ-032 | Controls | What does each mouse button and modifier combination do (for example Shift + left click to move a whole stack, Ctrl + left click to move one)? Defined per screen as screens are designed. | UI/Controls |
| OQ-033 | Controls | Should every drag have a click alternative (click an item to pick it up, click a destination to place it)? The Intermediate tier of the Game Accessibility Guidelines asks that "multiple simultaneous actions (eg. click/drag or swipe) are not required". Proposal: yes. | UI/Controls |
| OQ-058 | Runs | What perks, boosts and challenge unlocks can survival and challenge points buy, and at what prices? Deferred to a dedicated session. | Systems/Runs |
| OQ-064 | UI | Day summary: when exactly does it appear (at midnight, or when the survivor next wakes), and what does it show? Proposal in Design/UI/DaySummary. | UI/DaySummary |
| OQ-067 | Combat | Injuries in battle, continued: (1) do injuries the survivor already had before the battle still affect it (for example, a tool-side arm injury lowering damage dealt); (2) does game text call current health "SHP" (and the injury ceiling "HHP"), or "Health" as the HUD does? | Systems/Combat |

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
