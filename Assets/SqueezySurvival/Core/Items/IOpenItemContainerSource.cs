namespace SqueezySurvival.Core.Items
{
    /// <summary>Tells which container is open (Docs/Design/Systems/Containers.md, Opening).</summary>
    public interface IOpenItemContainerSource
    {
        /// <summary>The container that is open now, or null when none is. Only one is open at a time.</summary>
        IOpenItemContainer Current { get; }
    }
}
