using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Items
{
    /// <summary>What a grid cell's tooltip shows: a title, a subtitle, a description, and detail lines. Immutable.</summary>
    public sealed class ThingTooltipContent
    {
        /// <summary>Creates tooltip content.</summary>
        public ThingTooltipContent(string title, string subtitle, string description, IReadOnlyList<string> detailLines)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Subtitle = subtitle ?? throw new ArgumentNullException(nameof(subtitle));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            DetailLines = detailLines ?? throw new ArgumentNullException(nameof(detailLines));
        }

        /// <summary>The thing's name.</summary>
        public string Title { get; }

        /// <summary>An item's category, or empty for things without one.</summary>
        public string Subtitle { get; }

        /// <summary>A sentence or two about it.</summary>
        public string Description { get; }

        /// <summary>Facts such as weights, one per line.</summary>
        public IReadOnlyList<string> DetailLines { get; }
    }
}
