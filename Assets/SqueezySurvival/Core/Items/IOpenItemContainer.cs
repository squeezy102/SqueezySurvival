namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// The container that is open: where things can be moved to and from, besides the ground and the inventory
    /// (Docs/Design/Systems/Containers.md, Docs/Design/UI/DragAndDrop.md, Destinations).
    /// </summary>
    public interface IOpenItemContainer
    {
        /// <summary>The panel that shows this container.</summary>
        ItemPanel Panel { get; }

        /// <summary>The container's name, which the words of a move into it use.</summary>
        string Name { get; }

        /// <summary>The slots of the container.</summary>
        CountedCollection<ItemDefinition> Contents { get; }
    }
}
