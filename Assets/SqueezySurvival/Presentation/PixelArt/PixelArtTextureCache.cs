using System;
using System.Collections.Generic;
using SqueezySurvival.Core.PixelArt;
using UnityEngine;

namespace SqueezySurvival.Presentation.PixelArt
{
    /// <summary>
    /// Turns Core's pixel images into textures with no smoothing, so every art pixel stays a sharp square when scaled
    /// (Docs/Design/ArtDirection.md), and keeps each one so it is made only once.
    /// </summary>
    public sealed class PixelArtTextureCache : IDisposable
    {
        private readonly Dictionary<string, Texture2D> _iconTextures = new Dictionary<string, Texture2D>();
        private readonly Dictionary<string, Texture2D> _navigationIconTextures = new Dictionary<string, Texture2D>();
        private readonly Dictionary<PixelFrameStyle, Texture2D> _frameTextures = new Dictionary<PixelFrameStyle, Texture2D>();
        private Texture2D _beachBackdrop;

        /// <summary>The placeholder beach backdrop, painted once.</summary>
        public Texture2D BeachBackdrop => _beachBackdrop ?? (_beachBackdrop = ToTexture(PlaceholderBeachScenePainter.Create(), "Beach backdrop"));

        /// <summary>The icon for the thing with the given definition id.</summary>
        public Texture2D Icon(string definitionId)
        {
            if (!_iconTextures.TryGetValue(definitionId, out Texture2D texture))
            {
                texture = ToTexture(PlaceholderIconCatalog.CreateIcon(definitionId), definitionId);
                _iconTextures.Add(definitionId, texture);
            }

            return texture;
        }

        /// <summary>The icon of a nav bar button, by its id in <see cref="NavigationIconCatalog"/>.</summary>
        public Texture2D NavigationIcon(string iconId)
        {
            if (!_navigationIconTextures.TryGetValue(iconId, out Texture2D texture))
            {
                texture = ToTexture(NavigationIconCatalog.CreateIcon(iconId), iconId);
                _navigationIconTextures.Add(iconId, texture);
            }

            return texture;
        }

        /// <summary>The nine-slice frame image for a style.</summary>
        public Texture2D Frame(PixelFrameStyle style)
        {
            if (style == null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            if (!_frameTextures.TryGetValue(style, out Texture2D texture))
            {
                texture = ToTexture(PixelFrameImageFactory.Create(style), "Frame");
                _frameTextures.Add(style, texture);
            }

            return texture;
        }

        /// <summary>Destroys every texture made, since textures made at runtime are not freed on their own.</summary>
        public void Dispose()
        {
            foreach (Texture2D texture in _iconTextures.Values)
            {
                UnityEngine.Object.Destroy(texture);
            }

            foreach (Texture2D texture in _navigationIconTextures.Values)
            {
                UnityEngine.Object.Destroy(texture);
            }

            foreach (Texture2D texture in _frameTextures.Values)
            {
                UnityEngine.Object.Destroy(texture);
            }

            if (_beachBackdrop != null)
            {
                UnityEngine.Object.Destroy(_beachBackdrop);
                _beachBackdrop = null;
            }

            _iconTextures.Clear();
            _navigationIconTextures.Clear();
            _frameTextures.Clear();
        }

        /// <summary>
        /// Makes a texture of a pixel image with no smoothing, for images that change, such as the day and night dial.
        /// The caller owns it and destroys it.
        /// </summary>
        public static Texture2D CreateTexture(PixelImage image, string name) => ToTexture(image, name);

        /// <summary>Replaces a texture's pixels with an image of the same size.</summary>
        public static void CopyInto(Texture2D texture, PixelImage image)
        {
            if (texture == null)
            {
                throw new ArgumentNullException(nameof(texture));
            }

            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            texture.SetPixels32(ToPixels(image));
            texture.Apply(false, false);
        }

        // Point filtering keeps pixels square when scaled (https://docs.unity3d.com/ScriptReference/FilterMode.Point.html).
        private static Texture2D ToTexture(PixelImage image, string name)
        {
            var texture = new Texture2D(image.Width, image.Height, TextureFormat.RGBA32, false)
            {
                name = name,
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
            };
            texture.SetPixels32(ToPixels(image));
            texture.Apply(false, false);
            return texture;
        }

        // Texture rows start at the bottom, so the image is flipped as it is copied.
        private static Color32[] ToPixels(PixelImage image)
        {
            var pixels = new Color32[image.Width * image.Height];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    PixelColor color = image[x, y];
                    pixels[((image.Height - 1 - y) * image.Width) + x] = new Color32(color.Red, color.Green, color.Blue, color.Alpha);
                }
            }

            return pixels;
        }
    }
}
