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

Run options change what a run earns ([Run options](#run-options)).

## Run options

Two independent checkboxes, chosen when starting a run. Either, both or neither can be ticked.

| Option | Effect | Points |
|---|---|---|
| Casual mode | Free saving and loading; death is not final ([Saving](#saving)). | **None.** The checkbox states this explicitly: "Casual runs earn no points." |
| Easy mode | The game is easier: lower penalties, slower decay, slower stat depletion and similar. | Greatly reduced, for both survival and challenge points. |

A run with both options earns no points.

Easy mode's exact multipliers and point share: [OQ-049](../OpenQuestions.md).

## Saving

### Standard runs

No save scumming, ever (resolves OQ-011).

- **Save and exit** in the Escape menu is the only saving the player sees. There are no manual saves and no save slots to load.
- **The run is recorded after every action.** Quitting any other way (closing the window, a crash, ending the process) resumes from the last completed action, so quitting can never undo an outcome. Precedent: Darkest Dungeon II saves automatically after each action and offers no manual saves ([Siliconera](https://www.siliconera.com/saves-in-darkest-dungeon-2-explained/)).
- **On death**, the run's save is deleted.

### Casual runs

Resolves OQ-048.

- Each casual run has its own save folder holding up to **5 save instances**.
- **Autosave** at the start of every day.
- **Manual save** any time, from the Escape menu.
- **Load** any of the run's saves at any time.
- **On death**, the run is not deleted: a dialog lists the run's saves and the player picks one to load.

When all 5 are in use: [OQ-050](../OpenQuestions.md).
