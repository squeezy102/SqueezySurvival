# Open Questions

Unresolved design decisions. When one is resolved, record the answer in the owning document, then remove it from here.

| ID | Area | Question | Owner doc |
|---|---|---|---|
| OQ-004 | Stats | Stat ranges, decay rates per time unit, thresholds and their effects. | Systems/SurvivalStats |
| OQ-005 | Research | Does research progress in the background while other actions are performed, or does it occupy the survivor's time? | Systems/Research (planned) |
| OQ-006 | Research | Can multiple research items progress at once, or is there a queue? | Systems/Research (planned) |
| OQ-029 | Inventory | Weight balancing: the personal weight limit, carrier capacities, item weights, and whether load can exceed 100%. Weight is a delicate balancing lever affecting carrying, exploration, strain, food logistics and crafting. **Deferred to a dedicated weight balancing session.** | Systems/Inventory |
| OQ-031 | UI | Event log behavior. Proposal: shows the last few entries and fades when idle (auto-hide is a setting, on by default); clicking it opens the full scrollable history for the run, filterable by type (actions, finds, warnings). | UI/HUD |
| OQ-025 | Exploration | How does exploring work at Dawn, Dusk and Night (light level penalties, hazard and injury chances)? | Systems/Exploration |
| OQ-008 | Zones | How large is the island at MVP: how many beach zones, and how are they connected? | Systems/World (planned) |
| OQ-009 | Weather | Which weather states exist and how do they affect stats and actions? | Systems/Weather (planned) |
| OQ-010 | Failure | What ends a run (death conditions) and what persists between runs? | GameDesignDocument |
| OQ-011 | Saving | Save model: manual, autosave, single slot, or permadeath? | GameDesignDocument |

Resolved: OQ-012 and OQ-013 (see [ADR-0004](../Decisions/ADR-0004-Distribution-And-Self-Update.md)), OQ-014 and OQ-015 (see [Launcher](Launcher.md)), OQ-001, OQ-002, OQ-016, OQ-017 and OQ-018 (see [Time](Systems/Time.md)), OQ-019 and OQ-020 (see [HUD](UI/HUD.md)), OQ-003, OQ-021 and OQ-022 (see [Food](Systems/Food.md)), OQ-007 and OQ-023 (see [Exploration](Systems/Exploration.md)), OQ-024 (see [Resource Nodes](Systems/ResourceNodes.md)), OQ-026 and OQ-027 (see [Decay](Systems/Decay.md)), OQ-028 (see [Ground Pool](Systems/GroundPool.md)), OQ-030 (see [Exertion](Systems/Exertion.md)). IDs are never reused.
