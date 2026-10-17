using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Icons
{
    /// <summary>A UI element that draws the placeholder icon for one item or zone feature.</summary>
    public sealed class PlaceholderIconElement : VisualElement
    {
        private readonly string _definitionId;

        /// <summary>Creates an icon for the thing with the given definition id.</summary>
        public PlaceholderIconElement(string definitionId)
        {
            _definitionId = definitionId;
            pickingMode = PickingMode.Ignore;
            generateVisualContent += context => PlaceholderIconPainter.Paint(context.painter2D, contentRect, _definitionId);

            // Redraw at the new size whenever the cell is resized.
            RegisterCallback<GeometryChangedEvent>(_ => MarkDirtyRepaint());
        }
    }
}
