using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>The wireframe's combination rules: crafting is not designed yet, so nothing combines.</summary>
    public sealed class NoThingCombinations : IThingCombinationRules
    {
        /// <inheritdoc />
        public bool TryCombine(ICountableThingDefinition dragged, ICountableThingDefinition target, out string outcomeDescription)
        {
            outcomeDescription = null;
            return false;
        }
    }
}
