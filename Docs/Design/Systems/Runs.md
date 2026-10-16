# Runs

**Status:** Concept

A run is one survivor's life, from waking on the beach to death. What a run earns persists between runs.

## Death ends the run

Death ends the run (resolves OQ-010). In a standard run the save is deleted; in casual mode it is kept ([Saving](#saving)). Causes identified so far:

| Cause | Owner |
|---|---|
| The injury ceiling reaching zero | [Injuries and Health](Injuries.md#health) |
| Illness | Illness (planned) |
| Freezing | [Survival Stats: body temperature](SurvivalStats.md#body-temperature) |

## Points earned

When a run ends, it awards two currencies, based on how long the survivor lived.

| Currency | Earned per | Spent on |
|---|---|---|
| Survival points | Day survived | Perks and boosts that make the game easier or change how it plays. Ideas: character unlocks, cosmetics, theme packs. |
| Challenge points | Month survived (30 days) | Unlocks that make the game harder. |

Amounts and what can be bought: [OQ-047](../OpenQuestions.md). Detailed design is deferred.

## Saving

Each run has one save (resolves OQ-011).

| Save | When |
|---|---|
| Autosave | At the start of each new day (midnight). |
| Manual save | Any time, from the Escape menu. |

**On death**, the run's save is deleted, unless the player ticked **Casual mode** when starting the run. In casual mode the save survives death, so the player can load it and carry on.

Casual mode and points, and quitting to undo a bad outcome: [OQ-048](../OpenQuestions.md).
