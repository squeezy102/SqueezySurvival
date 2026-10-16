# Survival Stats

**Status:** Concept

Survival stats (also called **macros**) are the survivor's core conditions, each shown as a bar.

## Conventions

1. **Higher is always better.** Every bar fills as the condition improves: drinking fills the water bar, eating fills the food bar, sleeping fills the rest bar.
2. **Every bar has end-cap icons** showing what empty and full mean. The player never has to wonder which direction is good.
3. **Range 0 to 100**, stored with fractional precision so per-minute changes accumulate correctly ([Time: rates](Time.md#rates)).

## Stats

| Stat | Empty icon | Full icon | Notes |
|---|---|---|---|
| Food | Small stomach | Full stomach | One bar. Restored by eating, limited by per-food [tolerance](Food.md#food-tolerance) (resolves OQ-003). |
| Water | To be designed | To be designed | |
| Stamina | To be designed | To be designed | Short-term physical exertion. Drained by chaining physical actions; recovered by Wait and Rest. Below 50%, strenuous actions risk injury; at 0 the survivor passes out ([Exertion](Exertion.md)). |
| Wakefulness | To be designed | To be designed | Long-term need for sleep. Drained by time awake; restored by Nap and Sleep. Going too long without sleep causes sanity loss and other penalties. |
| Body temperature | - | - | Not a bar. Shown as a HUD icon. See [Body temperature](#body-temperature). |
| Sanity | Swirl | Peace sign | Drained while any stat is dangerous ([Actions: condition gates](Actions.md#condition-gates)). |
| Contentment | Frown | Smile | Lowered by repeating the same kind of work day after day ([Exertion: repetition](Exertion.md#repetition)). |
| Cleanliness | To be designed | To be designed | Starts low: the survivor wakes filthy. |

## Body temperature

Body temperature is the one stat where the middle is best: too cold and too hot are both dangerous. It is therefore not shown as a bar: it is a "thermometer and body" icon in the [HUD](../UI/HUD.md) static status area (resolves OQ-019).

- **Overheating:** demanding actions in high heat raise body temperature.
- **Freezing:** staying sedentary in cold temperatures lowers it, and can kill.
- Ambient temperature comes from the day phase and weather ([Time: day phases](Time.md#day-phases)).

The stat list is not final. Names, icons, decay rates and effects are specified as each stat is designed.

## Bands (draft)

Each stat is in one band at a time. Bands drive gates and penalties; for example, any stat in Dangerous blocks sleep.

| Band | Range |
|---|---|
| Good | 75-100 |
| Fair | 50-74 |
| Low | 25-49 |
| Dangerous | 10-24 |
| Critical | 0-9 |

Draft starting values, to be tuned in play. Bands may differ per stat.

## Open questions

[OQ-004](../OpenQuestions.md).
