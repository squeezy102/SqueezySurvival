namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// The arrival-stacking choice for a run, held in memory. Starts on <see cref="ItemArrivalStacking.AutoStack"/>;
    /// saving and the Options screen arrive with the settings system (Docs/Design/UI/Options.md).
    /// </summary>
    public sealed class ItemArrivalStackingSetting : IItemArrivalStackingSetting
    {
        /// <inheritdoc />
        public ItemArrivalStacking Stacking { get; set; } = ItemArrivalStacking.AutoStack;
    }
}
