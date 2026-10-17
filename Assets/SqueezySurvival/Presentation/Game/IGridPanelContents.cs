using System;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// What a <see cref="GridPanelView"/> shows and acts on: an ordered collection in Core, seen without its item
    /// type so one view works for items and zone features alike.
    /// </summary>
    public interface IGridPanelContents
    {
        /// <summary>Raised after the contents or their order change.</summary>
        event Action Changed;

        /// <summary>How many cells there are.</summary>
        int Count { get; }

        /// <summary>The icon id of the thing in a cell.</summary>
        string DefinitionIdAt(int index);

        /// <summary>The count badge of a cell: empty for a single thing.</summary>
        string CountBadgeAt(int index);

        /// <summary>The hover tooltip of a cell.</summary>
        ThingTooltipContent TooltipAt(int index);

        /// <summary>What releasing the cell at <paramref name="sourceIndex"/> over <paramref name="target"/> would do.</summary>
        GridDropOutcome ResolveDrop(int sourceIndex, GridDropTarget target);

        /// <summary>Carries out an outcome returned by <see cref="ResolveDrop"/> for the same source cell.</summary>
        void ApplyDrop(int sourceIndex, GridDropOutcome outcome);
    }
}
