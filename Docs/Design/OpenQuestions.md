# Open Questions

Unresolved design decisions. When one is resolved, record the answer in the owning document, then remove it from here.

| ID | Area | Question | Owner doc |
|---|---|---|---|
| OQ-003 | Stats | How do Hunger and Satiation differ? (For example: Hunger = nutrition need, Satiation = stomach fullness.) | Systems/SurvivalStats |
| OQ-004 | Stats | Stat ranges, decay rates per time unit, thresholds and their effects. | Systems/SurvivalStats |
| OQ-019 | Stats | How is body temperature displayed, given that both extremes are bad? Proposal: a centered bar with a cold icon left, a hot icon right and a highlighted comfort zone in the middle, as the one documented exception to "higher is better". | Systems/SurvivalStats |
| OQ-005 | Research | Does research progress in the background while other actions are performed, or does it occupy the survivor's time? | Systems/Research (planned) |
| OQ-006 | Research | Can multiple research items progress at once, or is there a queue? | Systems/Research (planned) |
| OQ-007 | Exploration | Is exploration yield random, weighted by zone, or depleting over time? | Systems/Exploration (planned) |
| OQ-008 | Zones | How large is the island at MVP: how many beach zones, and how are they connected? | Systems/World (planned) |
| OQ-009 | Weather | Which weather states exist and how do they affect stats and actions? | Systems/Weather (planned) |
| OQ-010 | Failure | What ends a run (death conditions) and what persists between runs? | GameDesignDocument |
| OQ-011 | Saving | Save model: manual, autosave, single slot, or permadeath? | GameDesignDocument |

Resolved: OQ-012 and OQ-013 (see [ADR-0004](../Decisions/ADR-0004-Distribution-And-Self-Update.md)), OQ-014 and OQ-015 (see [Launcher](Launcher.md)), OQ-001, OQ-002, OQ-016, OQ-017 and OQ-018 (see [Time](Systems/Time.md)). IDs are never reused.
