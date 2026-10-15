# Food

**Status:** Draft

Eating restores the Food survival stat ([Survival Stats](SurvivalStats.md)). Food satiety stops any single food from sustaining the survivor indefinitely.

## Food satiety

Every food type has its own satiety meter, from 0% to 100%. Eating a food raises its meter; time lowers it. A high meter means the survivor is sick of that food.

Purpose: the player cannot survive the whole game on coconuts. Variety is required.

### Effects by satiety

| Satiety before eating | Food restored | Contentment | Sanity | Other |
|---|---|---|---|---|
| 0-49% | Full | - | - | - |
| 50-74% | Diminishing | Penalty | - | - |
| 75-99% | Diminishing | Penalty | Penalty | - |
| 100% | None | - | - | Survivor becomes ill. Eating that food is unavailable ("can't stomach another") until satiety falls below 100%. |

Illness is specified in its own system (planned).

### Formulas (proposed, not approved)

| Quantity | Proposal | Effect |
|---|---|---|
| Satiety gained per serving | Set per food; coconut +15% | 4 coconuts in a row reach the diminishing threshold. |
| Satiety recovery | -1% per hour for every food | A food at 100% recovers to 0% in a little over 4 days; at 50%, in about 2 days. |
| Food restored multiplier | 100% below 50% satiety; falls linearly from 100% at 50% satiety to 25% at 100% satiety | `multiplier = 1 - 0.75 x (satiety - 50) / 50` for satiety from 50 to 100. |
| Contentment penalty | Set per food; scales with satiety | Values set during balancing. |
| Sanity penalty | Set per food; scales with satiety | Values set during balancing. |

### Display

Each food shows its satiety wherever the player chooses to eat it, with the current effect stated in words (for example "Getting sick of coconuts: restores 70%"). The meter follows the [color rule](../GameDesignDocument.md#pillars): color is never the only signal.
