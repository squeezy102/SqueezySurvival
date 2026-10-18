using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Icons
{
    /// <summary>
    /// A downward-pointing triangle marking a button that opens a menu. Drawn with UI Toolkit's vector API so it
    /// does not depend on the font having an arrow character.
    /// </summary>
    public sealed class DropdownArrowIconElement : VisualElement
    {
        /// <summary>Creates the arrow; it takes the text color of its button.</summary>
        public DropdownArrowIconElement()
        {
            pickingMode = PickingMode.Ignore;
            generateVisualContent += Paint;
            RegisterCallback<GeometryChangedEvent>(_ => MarkDirtyRepaint());
        }

        private void Paint(MeshGenerationContext context)
        {
            Rect area = contentRect;
            float size = Mathf.Min(area.width, area.height);
            if (size <= 0f)
            {
                return;
            }

            float halfWidth = size * 0.4f;
            float halfHeight = size * 0.25f;
            Vector2 center = area.center;
            Painter2D painter = context.painter2D;
            painter.fillColor = resolvedStyle.color;
            painter.BeginPath();
            painter.MoveTo(new Vector2(center.x - halfWidth, center.y - halfHeight));
            painter.LineTo(new Vector2(center.x + halfWidth, center.y - halfHeight));
            painter.LineTo(new Vector2(center.x, center.y + halfHeight));
            painter.ClosePath();
            painter.Fill();
        }
    }
}
