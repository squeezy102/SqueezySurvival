namespace SqueezySurvival.Core.Items
{
    /// <summary>Says no container is open, as is always so until containers are built.</summary>
    public sealed class NoOpenItemContainerSource : IOpenItemContainerSource
    {
        /// <inheritdoc />
        public IOpenItemContainer Current => null;
    }
}
