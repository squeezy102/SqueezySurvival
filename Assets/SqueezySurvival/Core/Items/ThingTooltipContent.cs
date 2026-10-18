using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Items
{
    /// <summary>What a grid cell's tooltip shows: a title, a subtitle, a description, and detail lines. Immutable.</summary>
    public sealed class ThingTooltipContent
    {
        /// <summary>Creates tooltip content.</summary>
        public ThingTooltipContent(string title, string subtitle, string description, IReadOnlyList<string> detailLines)
            : this(title, subtitle, description, detailLines, string.Empty)
        {
        }

        /// <summary>Creates tooltip content with an icon beside the title.</summary>
        /// <param name="iconId">The thing's icon id, or empty for no icon.</param>
        public ThingTooltipContent(string title, string subtitle, string description, IReadOnlyList<string> detailLines, string iconId)
        {
            IconId = iconId ?? throw new ArgumentNullException(nameof(iconId));
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

        /// <summary>The icon shown beside the title, or empty for none (Docs/Design/UI/InventoryGrids.md, Tooltips).</summary>
        public string IconId { get; }

        /// <summary>Facts such as weights, one per line.</summary>
        public IReadOnlyList<string> DetailLines { get; }
    }
}
