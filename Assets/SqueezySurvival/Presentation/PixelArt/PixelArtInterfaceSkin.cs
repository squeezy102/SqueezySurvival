using System;
using SqueezySurvival.Core.PixelArt;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.PixelArt
{
    /// <summary>
    /// Dresses interface elements in pixel-art frames (Docs/Design/ArtDirection.md, Interface): panels, windows,
    /// tooltips, slots and buttons. Frames are nine-sliced, so they stretch to any size without distorting
    /// (https://docs.unity3d.com/Manual/UIB-styling-ui-backgrounds.html).
    /// </summary>
    public sealed class PixelArtInterfaceSkin
    {
        private readonly PixelArtTextureCache _textures;

        /// <summary>Creates a skin that draws its frames from the given cache.</summary>
        public PixelArtInterfaceSkin(PixelArtTextureCache textures)
        {
            _textures = textures ?? throw new ArgumentNullException(nameof(textures));
        }

        /// <summary>The textures the skin draws from, for icons shown alongside it.</summary>
        public PixelArtTextureCache Textures => _textures;

        /// <summary>Frames every panel, window, tooltip and button under <paramref name="root"/>.</summary>
        public void ApplyTo(VisualElement root)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            root.Query(className: "panel").ForEach(element => ApplyFrame(element, InterfaceFrameStyles.Panel));
            root.Query(className: "pixel-window").ForEach(element => ApplyFrame(element, InterfaceFrameStyles.Window));
            root.Query(className: "grid-tooltip").ForEach(element => ApplyFrame(element, InterfaceFrameStyles.Tooltip));
            root.Query<Button>().ForEach(ApplyButton);
        }

        /// <summary>Shows the beach backdrop behind <paramref name="screenRoot"/>, cropped to fill it.</summary>
        public void ApplyBackdrop(VisualElement screenRoot)
        {
            if (screenRoot == null)
            {
                throw new ArgumentNullException(nameof(screenRoot));
            }

            screenRoot.style.backgroundImage = Background.FromTexture2D(_textures.BeachBackdrop);
        }

        /// <summary>Draws a frame as the element's background, replacing its color, borders and rounded corners.</summary>
        public void ApplyFrame(VisualElement element, PixelFrameStyle style)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (style == null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            element.style.backgroundImage = Background.FromTexture2D(_textures.Frame(style));
            element.style.backgroundColor = Color.clear;
            element.style.unitySliceLeft = style.BorderThickness;
            element.style.unitySliceRight = style.BorderThickness;
            element.style.unitySliceTop = style.BorderThickness;
            element.style.unitySliceBottom = style.BorderThickness;
            element.style.unitySliceScale = InterfaceFrameStyles.PixelScale;
            element.style.borderLeftWidth = 0f;
            element.style.borderRightWidth = 0f;
            element.style.borderTopWidth = 0f;
            element.style.borderBottomWidth = 0f;
            element.style.borderTopLeftRadius = 0f;
            element.style.borderTopRightRadius = 0f;
            element.style.borderBottomLeftRadius = 0f;
            element.style.borderBottomRightRadius = 0f;
        }

        /// <summary>
        /// Frames a button as a raised block that lightens while hovered or focused and sinks while pressed. Inline
        /// styles can't follow pointer states the way style sheets do, so the frame is swapped on pointer and focus events.
        /// </summary>
        public void ApplyButton(Button button)
        {
            if (button == null)
            {
                throw new ArgumentNullException(nameof(button));
            }

            var state = new ButtonPointerState();
            void Refresh() => ApplyFrame(button, state.FrameStyle);

            button.RegisterCallback<PointerEnterEvent>(_ => { state.Hovered = true; Refresh(); });
            button.RegisterCallback<PointerLeaveEvent>(_ => { state.Hovered = false; state.Pressed = false; Refresh(); });
            button.RegisterCallback<FocusInEvent>(_ => { state.Focused = true; Refresh(); });
            button.RegisterCallback<FocusOutEvent>(_ => { state.Focused = false; Refresh(); });

            // The button's own click handling captures the pointer, so these listen as the event trickles down.
            button.RegisterCallback<PointerDownEvent>(_ => { state.Pressed = true; Refresh(); }, TrickleDown.TrickleDown);
            button.RegisterCallback<PointerUpEvent>(_ => { state.Pressed = false; Refresh(); }, TrickleDown.TrickleDown);
            Refresh();
        }

        private sealed class ButtonPointerState
        {
            public bool Hovered { get; set; }

            public bool Focused { get; set; }

            public bool Pressed { get; set; }

            public PixelFrameStyle FrameStyle =>
                Pressed ? InterfaceFrameStyles.PressedButton
                : Hovered || Focused ? InterfaceFrameStyles.HoveredButton
                : InterfaceFrameStyles.Button;
        }
    }
}
