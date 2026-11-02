namespace SqueezySurvival.Core.Items
{
    /// <summary>The player's current choice of <see cref="ItemArrivalStacking"/>, read each time something moves.</summary>
    public interface IItemArrivalStackingSetting
    {
        /// <summary>How things moved into a panel are stacked.</summary>
        ItemArrivalStacking Stacking { get; }
    }
}
