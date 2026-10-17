using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Options
{
    /// <summary>Makes a template instance cover its parent, so an overlay inside it can fill the screen.</summary>
    public static class OverlayElementStyler
    {
        /// <summary>
        /// A UXML template instance is laid out like any child element; positioning it absolutely at all four edges
        /// lets the overlay it holds cover the whole screen instead of taking space in the layout.
        /// </summary>
        public static void CoverParent(VisualElement templateInstance)
        {
            templateInstance.style.position = Position.Absolute;
            templateInstance.style.left = 0;
            templateInstance.style.top = 0;
            templateInstance.style.right = 0;
            templateInstance.style.bottom = 0;
        }
    }
}
