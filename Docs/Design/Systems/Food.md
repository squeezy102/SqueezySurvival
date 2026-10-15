# Food

**Status:** Draft

**Design intent: the survivor must eat a balanced diet.** Every rule and value in this document serves that goal; tuning is judged against it.

Eating restores the Food survival stat ([Survival Stats](SurvivalStats.md)). Food tolerance stops any single food, or any single category, from sustaining the survivor indefinitely.

## Food tolerance

Tolerance is tracked at two levels (resolves OQ-022):

- **Food tolerance:** one meter per individual food (apple, banana, coconut).
- **Category tolerance:** one meter per food category (Fruit, Meat, Fish).

Every serving lowers both: its own food's meter sharply, and its category's meter by a fraction of that. Meters run from 100% (happy to eat it) to 0% (cannot stomach it) and read higher-is-better like every other bar (resolves OQ-021). They recover over time (fully within a week) and slightly whenever the survivor eats something else.

- Five apples in a row: apple tolerance drops sharply; Fruit tolerance drops only a little.
- Five each of apples, bananas and grapes: each food drops sharply, and together they drive Fruit tolerance down. Persisting makes every fruit sicken the survivor until Fruit tolerance recovers.

**Food is never locked.** The survivor can always eat anything they have. The cost of a poor diet comes from reduced food restored, contentment and sanity penalties, and illness.

**Governing tolerance:** a serving's effects use the lower of the food's tolerance and its category's tolerance.

Purpose: the player cannot survive the whole game on coconuts. Variety is required.

### Effects by tolerance

The governing tolerance is checked before each serving.

| Tolerance | Food restored | Contentment | Sanity | Other |
|---|---|---|---|---|
| 51-100% | Full | - | - | - |
| 26-50% | Diminishing | Penalty | - | - |
| 1-25% | Diminishing | Penalty | Penalty | - |
| 0% | None: the survivor cannot keep it down | - | - | Survivor becomes ill. Illness brings its own penalties, such as dehydration and fatigue, and can kill. |

Illness is specified in its own system (planned).

### Formulas (proposed, not approved)

| Quantity | Proposal | Effect |
|---|---|---|
| Tolerance lost per serving | Set per food; coconut -15% | The 4th coconut in a row brings tolerance to the diminishing threshold. |
| Category tolerance lost per serving | One third of the food's loss; coconut -5% Fruit | 5 coconuts: coconut 25%, Fruit 75%. 5 each of three fruits (15 servings): Fruit 25%. About 20 fruit servings in a row bring Fruit to 0%, where every fruit makes the survivor ill. |
| Tolerance recovery over time | 100% per week: about +0.6% per hour (100 / 168) for every food and category | A food or category at 0% fully recovers after a week without eating it; from 50%, in about 3.5 days. |
| Tolerance recovery from variety | Each serving restores +1% to every other food and every other category | Eating a varied diet speeds recovery, but less than time: six varied servings a day add about 6%, against about 14% a day from time. |
| Food restored multiplier | 100% above 50% tolerance; falls linearly from 100% at 50% tolerance to 25% just above 0%; 0% at 0% | `multiplier = 0.25 + 0.75 x tolerance / 50` for tolerance above 0 and up to 50. |
| Contentment penalty | Set per food; grows as tolerance falls | Values set during balancing. |
| Sanity penalty | Set per food; grows as tolerance falls | Values set during balancing. |

### Display

**Food Tolerance window.** Clicking the Food bar opens a window of tolerance meters grouped by food category.

- Categories: Meat, Fish, Fruit, Vegetables, Dairy, and others as foods are added. The category list is data.
- Each category expands to list its individual foods.
- Only foods the survivor has discovered or eaten are listed; categories with none are hidden ([You only know what you know](../GameDesignDocument.md#pillars)).
- Ordered from lowest tolerance to highest, so the foods to avoid are at the top. Each category row shows the category's own tolerance meter.
- Hovering a meter shows its exact value and current penalties.
- A side panel explains the thresholds (the table above) in plain language.
- Wherever the player chooses food, its tolerance and current effect are stated in words, for example "Getting tired of coconuts: restores 70%".
- Follows the [color rule](../GameDesignDocument.md#pillars): color is never the only signal.
