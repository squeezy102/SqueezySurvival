using System;

namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// Names a panel things move between: the ground, the inventory or a container (Docs/Design/UI/DragAndDrop.md,
    /// Destinations). A container is named by its id, so renaming it does not change which panel it is. Immutable.
    /// </summary>
    public readonly struct ItemPanel : IEquatable<ItemPanel>
    {
        private ItemPanel(ItemPanelKind kind, string containerId)
        {
            Kind = kind;
            ContainerId = containerId;
        }

        /// <summary>The current zone's ground.</summary>
        public static ItemPanel Ground { get; } = new ItemPanel(ItemPanelKind.Ground, null);

        /// <summary>The survivor's inventory.</summary>
        public static ItemPanel Inventory { get; } = new ItemPanel(ItemPanelKind.Inventory, null);

        /// <summary>What kind of panel this is.</summary>
        public ItemPanelKind Kind { get; }

        /// <summary>The id of the container this panel shows; null for the ground and the inventory.</summary>
        public string ContainerId { get; }

        /// <summary>The panel of the container with <paramref name="containerId"/>.</summary>
        public static ItemPanel ForContainer(string containerId)
        {
            if (string.IsNullOrWhiteSpace(containerId))
            {
                throw new ArgumentException("A container has an id.", nameof(containerId));
            }

            return new ItemPanel(ItemPanelKind.Container, containerId);
        }

        /// <summary>Whether two panels are the same panel.</summary>
        public static bool operator ==(ItemPanel left, ItemPanel right) => left.Equals(right);

        /// <summary>Whether two panels are different panels.</summary>
        public static bool operator !=(ItemPanel left, ItemPanel right) => !left.Equals(right);

        /// <inheritdoc />
        public bool Equals(ItemPanel other) => Kind == other.Kind && string.Equals(ContainerId, other.ContainerId, StringComparison.Ordinal);

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is ItemPanel other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Kind, ContainerId);

        /// <inheritdoc />
        public override string ToString() => Kind == ItemPanelKind.Container ? $"Container {ContainerId}" : Kind.ToString();
    }
}
