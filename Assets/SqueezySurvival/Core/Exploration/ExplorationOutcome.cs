using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Exploration
{
    /// <summary>What one explore turned up and how it changed the zone's progress. Immutable.</summary>
    public sealed class ExplorationOutcome
    {
        /// <summary>Creates an outcome.</summary>
        public ExplorationOutcome(
            IReadOnlyList<ItemDefinition> itemsFound,
            IReadOnlyList<ZoneFeatureDefinition> featuresFound,
            ExplorationProgress progressBefore,
            ExplorationProgress progressAfter)
        {
            ItemsFound = itemsFound ?? throw new ArgumentNullException(nameof(itemsFound));
            FeaturesFound = featuresFound ?? throw new ArgumentNullException(nameof(featuresFound));
            ProgressBefore = progressBefore ?? throw new ArgumentNullException(nameof(progressBefore));
            ProgressAfter = progressAfter ?? throw new ArgumentNullException(nameof(progressAfter));
        }

        /// <summary>Loose items found, one entry per item, in the order found.</summary>
        public IReadOnlyList<ItemDefinition> ItemsFound { get; }

        /// <summary>Features revealed, in the order found.</summary>
        public IReadOnlyList<ZoneFeatureDefinition> FeaturesFound { get; }

        /// <summary>The zone's progress before the explore.</summary>
        public ExplorationProgress ProgressBefore { get; }

        /// <summary>The zone's progress after the explore.</summary>
        public ExplorationProgress ProgressAfter { get; }

        /// <summary>Whether this explore is the one that mapped the zone fully.</summary>
        public bool CompletedTheZone => !ProgressBefore.IsFullyExplored && ProgressAfter.IsFullyExplored;
    }
}
