# Food

**Status:** Draft

Eating restores the Food survival stat ([Survival Stats](SurvivalStats.md)). Food tolerance stops any single food from sustaining the survivor indefinitely.

## Food tolerance

Every food type has its own tolerance meter, from 100% (happy to eat it) down to 0% (cannot stomach it). Eating a food lowers its tolerance; time restores it. The meter reads higher-is-better like every other bar (resolves OQ-021).

Purpose: the player cannot survive the whole game on coconuts. Variety is required.

### Effects by tolerance

Tolerance is checked before each serving.

| Tolerance | Food restored | Contentment | Sanity | Other |
|---|---|---|---|---|
| 51-100% | Full | - | - | - |
| 26-50% | Diminishing | Penalty | - | - |
| 1-25% | Diminishing | Penalty | Penalty | - |
| 0% | None | - | - | Survivor becomes ill. Eating that food is unavailable ("can't stomach another") until tolerance recovers above 0%. |

Illness is specified in its own system (planned).

### Formulas (proposed, not approved)

| Quantity | Proposal | Effect |
|---|---|---|
| Tolerance lost per serving | Set per food; coconut -15% | The 4th coconut in a row brings tolerance to the diminishing threshold. |
| Tolerance recovery | +1% per hour for every food | From 0% to 100% in a little over 4 days; from 50% in about 2 days. |
| Food restored multiplier | 100% above 50% tolerance; falls linearly from 100% at 50% tolerance to 25% at 0% | `multiplier = 0.25 + 0.75 x tolerance / 50` for tolerance from 0 to 50. |
| Contentment penalty | Set per food; grows as tolerance falls | Values set during balancing. |
| Sanity penalty | Set per food; grows as tolerance falls | Values set during balancing. |

### Display

**Food Tolerance window.** Clicking the Food bar opens a window of tolerance meters grouped by food category.

- Categories: Meat, Fish, Fruit, Vegetables, Dairy, and others as foods are added. The category list is data.
- Each category expands to list its individual foods.
- Only foods the survivor has discovered or eaten are listed; categories with none are hidden ([You only know what you know](../GameDesignDocument.md#pillars)).
- Ordered from lowest tolerance to highest, so the foods to avoid are at the top. Whether tolerance is tracked per food or per category: [OQ-022](../OpenQuestions.md).
- Hovering a meter shows its exact value and current penalties.
- A side panel explains the thresholds (the table above) in plain language.
- Wherever the player chooses food, its tolerance and current effect are stated in words, for example "Getting tired of coconuts: restores 70%".
- Follows the [color rule](../GameDesignDocument.md#pillars): color is never the only signal.
