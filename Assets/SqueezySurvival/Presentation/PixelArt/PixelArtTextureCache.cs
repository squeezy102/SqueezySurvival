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
        private readonly Dictionary<PixelFrameStyle, Texture2D> _frameTextures = new Dictionary<PixelFrameStyle, Texture2D>();

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

            foreach (Texture2D texture in _frameTextures.Values)
            {
                UnityEngine.Object.Destroy(texture);
            }

            _iconTextures.Clear();
            _frameTextures.Clear();
        }

        // Point filtering keeps pixels square when scaled (https://docs.unity3d.com/ScriptReference/FilterMode.Point.html).
        // Texture rows start at the bottom, so the image is flipped as it is copied.
        private static Texture2D ToTexture(PixelImage image, string name)
        {
            var texture = new Texture2D(image.Width, image.Height, TextureFormat.RGBA32, false)
            {
                name = name,
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
            };
            var pixels = new Color32[image.Width * image.Height];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    PixelColor color = image[x, y];
                    pixels[((image.Height - 1 - y) * image.Width) + x] = new Color32(color.Red, color.Green, color.Blue, color.Alpha);
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply(false, true);
            return texture;
        }
    }
}
