using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Decides whether one thing dropped on another combines into something (Docs/Design/UI/DragAndDrop.md,
    /// Combining). Recipes belong to crafting.
    /// </summary>
    public interface IThingCombinationRules
    {
        /// <summary>Whether <paramref name="dragged"/> dropped on <paramref name="target"/> combines, and into what.</summary>
        bool TryCombine(ICountableThingDefinition dragged, ICountableThingDefinition target, out string outcomeDescription);
    }
}
