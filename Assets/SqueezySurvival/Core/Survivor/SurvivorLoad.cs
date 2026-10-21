using System;

namespace SqueezySurvival.Core.Survivor
{
    /// <summary>
    /// What the survivor carries against their weight limit, and the state that puts them in
    /// (Docs/Design/Systems/Inventory.md, Load). Immutable.
    /// </summary>
    public sealed class SurvivorLoad
    {
        /// <summary>The personal weight limit: 100 lb, a placeholder until the weight balancing pass (OQ-087).</summary>
        public const decimal PlaceholderLimitPounds = 100m;

        /// <summary>The load, in percent, at which the survivor is overburdened.</summary>
        public const decimal OverburdenedFromPercent = 75m;

        /// <summary>The load, in percent, at which the survivor is immobile.</summary>
        public const decimal ImmobileFromPercent = 100m;

        /// <summary>A load of <paramref name="carriedPounds"/> against <paramref name="limitPounds"/>. There is no cap.</summary>
        public SurvivorLoad(decimal carriedPounds, decimal limitPounds)
        {
            if (carriedPounds < 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(carriedPounds), carriedPounds, "Cannot be negative.");
            }

            if (limitPounds <= 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(limitPounds), limitPounds, "Must be more than zero.");
            }

            CarriedPounds = carriedPounds;
            LimitPounds = limitPounds;
        }

        /// <summary>The weight carried, in pounds.</summary>
        public decimal CarriedPounds { get; }

        /// <summary>The personal weight limit, in pounds.</summary>
        public decimal LimitPounds { get; }

        /// <summary>The weight carried as a percentage of the limit; it can go past 100.</summary>
        public decimal Percent => CarriedPounds / LimitPounds * 100m;

        /// <summary>The state the load puts the survivor in.</summary>
        public SurvivorLoadState State =>
            Percent >= ImmobileFromPercent ? SurvivorLoadState.Immobile
            : Percent >= OverburdenedFromPercent ? SurvivorLoadState.Overburdened
            : SurvivorLoadState.Normal;
    }
}
