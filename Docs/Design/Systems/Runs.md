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
| Survival points | 1 per day survived | Perks and boosts that make the game easier or change how it plays. Ideas: character unlocks, cosmetics, theme packs. |
| Challenge points | 1 per month survived (30 days) | Unlocks that make the game harder. |

What points can buy: [OQ-058](../OpenQuestions.md). Detailed design is deferred.

Run options change what a run earns ([Run options](#run-options)).

## Run options

Checkboxes chosen when starting a run. Casual and easy mode can be combined; hard mode excludes both.

| Option | Effect | Points |
|---|---|---|
| Casual mode | Free saving and loading; death is not final ([Saving](#saving)). | **None.** The checkbox states this explicitly: "Casual runs earn no points." |
| Easy mode | The game is easier: lower penalties, slower decay, slower stat depletion and similar. | Survival points at 1/5 value: 1 per 5 days survived. **No challenge points.** |

| Hard mode | The game is harder: higher penalties, faster decay, faster stat depletion and similar. | Double: 2 survival points per day, 2 challenge points per 30 days (resolves OQ-051). |

| Combination | Allowed |
|---|---|
| Casual + Easy | Yes. Earns no points. |
| Hard + Casual, Hard + Easy | No: ticking hard mode clears and disables the other two, and the reverse. |

How much easier easy mode and harder hard mode are: [OQ-059](../OpenQuestions.md). All point rates are drafts, tuned during user acceptance testing.

## Saving

### Standard runs

No save scumming, ever (resolves OQ-011).

- **Save and exit** in the Escape menu is the only saving the player sees. There are no manual saves and no save slots to load.
- **The run is recorded after every action.** Quitting any other way (closing the window, a crash, ending the process) resumes from the last completed action, so quitting can never undo an outcome. Precedent: Darkest Dungeon II saves automatically after each action and offers no manual saves ([Siliconera](https://www.siliconera.com/saves-in-darkest-dungeon-2-explained/)).
- **On death**, the run's save is deleted.

### Casual runs

Resolves OQ-048 and OQ-050.

- Each casual run has its own save folder with **6 slots**:
  - **5 general slots**, each holding a manual save or an autosave.
  - **1 dedicated autosave slot**, always holding the most recent autosave.
- **Autosave** at the start of every day.
- **Manual save** any time, from the Escape menu.
- **Load** any of the run's saves at any time.
- **On death**, the run is not deleted: a dialog lists the run's saves and the player picks one to load.

| New save | Goes to |
|---|---|
| Autosave | Always the dedicated slot. Also a free general slot, or else the general slot with the oldest autosave. **Never replaces a manual save.** If all 5 general slots hold manual saves, only the dedicated slot is written. |
| Manual save | A free general slot, or else only the save the player chooses to overwrite. Nothing is replaced without the player's choice. |
