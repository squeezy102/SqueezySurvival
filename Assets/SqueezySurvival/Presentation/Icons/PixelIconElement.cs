using System;
using SqueezySurvival.Core.PixelArt;
using SqueezySurvival.Presentation.PixelArt;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Icons
{
    /// <summary>
    /// Shows a 16 x 16 pixel icon, such as an item's or a nav button's, centered in its parent at the largest whole-number
    /// scale that fits, so every art pixel is the same size (Docs/Design/ArtDirection.md, Icons).
    /// </summary>
    public sealed class PixelIconElement : VisualElement
    {
        // How much of the parent the icon may fill, leaving a little room around it.
        private const float ParentFraction = 0.9f;

        private VisualElement _observedParent;

        /// <summary>Creates an element showing the given icon texture.</summary>
        /// <param name="icon">The icon, from a <see cref="PixelArtTextureCache"/>.</param>
        /// <param name="fitsParent">Whether the icon sizes and centers itself in its parent; false when its owner places it.</param>
        public PixelIconElement(Texture2D icon, bool fitsParent = true)
        {
            if (icon == null)
            {
                throw new ArgumentNullException(nameof(icon));
            }

            pickingMode = PickingMode.Ignore;
            style.position = Position.Absolute;
            style.backgroundImage = Background.FromTexture2D(icon);
            if (fitsParent)
            {
                RegisterCallback<AttachToPanelEvent>(_ => ObserveParent());
                RegisterCallback<DetachFromPanelEvent>(_ => StopObservingParent());
            }
        }

        /// <summary>The largest whole multiple of the icon size that fits in <paramref name="available"/>, never under 1x.</summary>
        public static float WholePixelSize(float available)
        {
            int scale = Math.Max(1, (int)Math.Floor(available * ParentFraction / PlaceholderIconCatalog.IconSize));
            return scale * PlaceholderIconCatalog.IconSize;
        }

        /// <summary>Sizes the icon to a whole-pixel scale and centers it in an area of the given size.</summary>
        public void FitInto(float areaWidth, float areaHeight)
        {
            float size = WholePixelSize(Mathf.Min(areaWidth, areaHeight));
            style.width = size;
            style.height = size;
            style.left = (areaWidth - size) / 2f;
            style.top = (areaHeight - size) / 2f;
        }

        private void ObserveParent()
        {
            StopObservingParent();
            _observedParent = parent;
            _observedParent?.RegisterCallback<GeometryChangedEvent>(OnParentGeometryChanged);
        }

        private void StopObservingParent()
        {
            _observedParent?.UnregisterCallback<GeometryChangedEvent>(OnParentGeometryChanged);
            _observedParent = null;
        }

        private void OnParentGeometryChanged(GeometryChangedEvent changed) => FitInto(changed.newRect.width, changed.newRect.height);
    }
}
