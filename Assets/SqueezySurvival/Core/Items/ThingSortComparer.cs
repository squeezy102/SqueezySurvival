using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// Orders a grid's entries for sorting (Docs/Design/UI/InventoryGrids.md, Sorting). Things that are not items,
    /// such as zone features, have no category or weight, so they fall back to their names.
    /// </summary>
    public sealed class ThingSortComparer<TDefinition> : IComparer<CountedEntry<TDefinition>>
        where TDefinition : class, ICountableThingDefinition
    {
        private readonly ThingSortOrder _order;

        /// <summary>Creates a comparer for the given order.</summary>
        public ThingSortComparer(ThingSortOrder order)
        {
            if (!Enum.IsDefined(typeof(ThingSortOrder), order))
            {
                throw new ArgumentOutOfRangeException(nameof(order), order, "Unknown sort order.");
            }

            _order = order;
        }

        /// <inheritdoc />
        public int Compare(CountedEntry<TDefinition> first, CountedEntry<TDefinition> second)
        {
            if (first == null || second == null)
            {
                throw new ArgumentNullException(first == null ? nameof(first) : nameof(second));
            }

            int comparison;
            switch (_order)
            {
                case ThingSortOrder.ByType:
                    comparison = CategoryRank(first).CompareTo(CategoryRank(second));
                    break;
                case ThingSortOrder.ByQuantity:
                    comparison = second.Count.CompareTo(first.Count);
                    break;
                case ThingSortOrder.ByWeight:
                    comparison = StackWeight(second).CompareTo(StackWeight(first));
                    break;
                default:
                    comparison = 0;
                    break;
            }

            return comparison != 0
                ? comparison
                : string.Compare(first.Definition.Name, second.Definition.Name, StringComparison.OrdinalIgnoreCase);
        }

        private static int CategoryRank(CountedEntry<TDefinition> entry) =>
            entry.Definition is ItemDefinition item ? (int)item.Category : int.MaxValue;

        private static decimal StackWeight(CountedEntry<TDefinition> entry) =>
            entry.Definition is ItemDefinition item ? item.WeightPounds * entry.Count : 0m;
    }
}
