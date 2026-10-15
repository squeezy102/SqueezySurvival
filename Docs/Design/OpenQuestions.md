# Open Questions

Unresolved design decisions. When one is resolved, record the answer in the owning document, then remove it from here.

| ID | Area | Question | Owner doc |
|---|---|---|---|
| OQ-001 | Time | What real-world duration is one time unit (for example 15 minutes)? | Systems/Time (planned) |
| OQ-002 | Time | Does time advance only through actions, or is there also a wait/rest action to fast-forward? | Systems/Time (planned) |
| OQ-003 | Stats | How do Hunger and Satiation differ? (For example: Hunger = nutrition need, Satiation = stomach fullness.) | Systems/SurvivalStats (planned) |
| OQ-004 | Stats | Stat ranges, decay rates per time unit, thresholds and their effects. | Systems/SurvivalStats (planned) |
| OQ-005 | Research | Does research progress in the background while other actions are performed, or does it occupy the survivor's time? | Systems/Research (planned) |
| OQ-006 | Research | Can multiple research items progress at once, or is there a queue? | Systems/Research (planned) |
| OQ-007 | Exploration | Is exploration yield random, weighted by zone, or depleting over time? | Systems/Exploration (planned) |
| OQ-008 | Zones | How large is the island at MVP: how many beach zones, and how are they connected? | Systems/World (planned) |
| OQ-009 | Weather | Which weather states exist and how do they affect stats and actions? | Systems/Weather (planned) |
| OQ-010 | Failure | What ends a run (death conditions) and what persists between runs? | GameDesignDocument |
| OQ-011 | Saving | Save model: manual, autosave, single slot, or permadeath? | GameDesignDocument |
| OQ-014 | Launcher | After Play, does the launcher close, minimize, or stay open? Recommendation: close, so updates are never attempted while game files are in use. | Launcher |
| OQ-015 | Distribution | Is an update control also needed inside the game, or is the launcher the only place updates happen? | Launcher |

Resolved: OQ-012 and OQ-013 (see [ADR-0004](../Decisions/ADR-0004-Distribution-And-Self-Update.md)). IDs are never reused.
