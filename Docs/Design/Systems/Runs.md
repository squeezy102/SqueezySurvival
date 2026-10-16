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

No save scumming, ever, outside casual mode (resolves OQ-011).

- **Save and exit** in the Escape menu is the only saving the player sees. There are no manual saves and no save slots to load.
- **The run is recorded after every action.** Quitting any other way (closing the window, a crash, ending the process) resumes from the last completed action, so quitting can never undo an outcome. Precedent: Darkest Dungeon II saves automatically after each action and offers no manual saves ([Siliconera](https://www.siliconera.com/saves-in-darkest-dungeon-2-explained/)).
- **On death**, the run's save is deleted, unless the player ticked **Casual mode** when starting the run.

Casual mode's rules (what it restores after death, whether it allows manual saves, and its points): [OQ-048](../OpenQuestions.md).
