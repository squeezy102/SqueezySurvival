# Open Questions

Unresolved design decisions. When one is resolved, record the answer in the owning document, then move its ID to the resolved table.

| ID | Area | Question | Owner doc |
|---|---|---|---|
| OQ-004 | Stats | Stat ranges, decay rates per time unit, thresholds and their effects. | Systems/SurvivalStats |
| OQ-008 | Zones | How large is the island at MVP: how many beach zones, and how are they connected? | Systems/Zones |
| OQ-009 | Weather | Which weather states exist and how do they affect stats and actions? | Systems/Weather (planned) |
| OQ-010 | Failure | What ends a run (death conditions) and what persists between runs? | GameDesignDocument |
| OQ-011 | Saving | Save model: manual, autosave, single slot, or permadeath? | GameDesignDocument |
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

## Resolved

IDs are never reused.

| IDs | Recorded in |
|---|---|
| OQ-001, OQ-002, OQ-016, OQ-017, OQ-018 | [Time](Systems/Time.md) |
| OQ-003, OQ-021, OQ-022 | [Food](Systems/Food.md) |
| OQ-005, OQ-006 | [Research](Systems/Research.md) |
| OQ-007, OQ-023 | [Exploration](Systems/Exploration.md) |
| OQ-012, OQ-013 | [ADR-0004](../Decisions/ADR-0004-Distribution-And-Self-Update.md) |
| OQ-014, OQ-015 | [Launcher](Launcher.md) |
| OQ-019, OQ-020, OQ-031, OQ-034 | [HUD](UI/HUD.md) |
| OQ-024 | [Resource Nodes](Systems/ResourceNodes.md) |
| OQ-026, OQ-027 | [Decay](Systems/Decay.md) |
| OQ-028 | [Zones](Systems/Zones.md) |
| OQ-030 | [Exertion](Systems/Exertion.md) |
| OQ-035 | [Screen Layout](UI/ScreenLayout.md) |
| OQ-039, OQ-041, OQ-042 | [Combat](Systems/Combat.md) |
| OQ-045 | [Encounters](Systems/Encounters.md) |
