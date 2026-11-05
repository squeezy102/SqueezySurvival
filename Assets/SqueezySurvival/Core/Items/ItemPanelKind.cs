namespace SqueezySurvival.Core.Items
{
    /// <summary>The kinds of panel things move between (Docs/Design/UI/DragAndDrop.md, Destinations).</summary>
    public enum ItemPanelKind
    {
        /// <summary>The current zone's ground.</summary>
        Ground,

        /// <summary>The survivor's inventory.</summary>
        Inventory,

        /// <summary>The open container.</summary>
        Container,
    }
}
