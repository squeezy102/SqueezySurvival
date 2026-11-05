using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Items
{
    /// <summary>
    /// A container for tests, until containers are built: it is both the open container and the source that says it is
    /// open, and a test can close it.
    /// </summary>
    public sealed class StandInOpenItemContainerSource : IOpenItemContainerSource, IOpenItemContainer
    {
        /// <summary>The name the stand-in container gives its moves.</summary>
        public const string ContainerName = "woven palm basket";

        /// <summary>Creates an open, empty container with <paramref name="slotCapacity"/> slots.</summary>
        public StandInOpenItemContainerSource(int slotCapacity = 8)
        {
            Contents = new CountedCollection<ItemDefinition>(slotCapacity);
        }

        /// <summary>Whether the container is open. Tests close it by setting this to false.</summary>
        public bool IsOpen { get; set; } = true;

        /// <inheritdoc />
        public ItemPanel Panel { get; } = ItemPanel.ForContainer("stand-in-basket");

        /// <inheritdoc />
        public string Name => ContainerName;

        /// <inheritdoc />
        public CountedCollection<ItemDefinition> Contents { get; }

        /// <inheritdoc />
        public IOpenItemContainer Current => IsOpen ? this : null;
    }
}
