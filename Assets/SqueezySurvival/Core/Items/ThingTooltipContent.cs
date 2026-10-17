using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Items
{
    /// <summary>What a grid cell's tooltip shows: a title, a description, and detail lines. Immutable.</summary>
    public sealed class ThingTooltipContent
    {
        /// <summary>Creates tooltip content.</summary>
        public ThingTooltipContent(string title, string description, IReadOnlyList<string> detailLines)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            DetailLines = detailLines ?? throw new ArgumentNullException(nameof(detailLines));
        }

        /// <summary>The thing's name.</summary>
        public string Title { get; }

        /// <summary>A sentence or two about it.</summary>
        public string Description { get; }

        /// <summary>Facts such as weights, one per line.</summary>
        public IReadOnlyList<string> DetailLines { get; }
    }
}
