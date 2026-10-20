using System;
using NUnit.Framework;
using SqueezySurvival.Core.PixelArt;

namespace SqueezySurvival.Core.Tests.PixelArt
{
    [TestFixture]
    public sealed class StatusIconCatalogTests
    {
        [Test]
        public void CreateIcon_Warning_IsAnIconSizedImageWithPaintedPixels()
        {
            PixelImage icon = StatusIconCatalog.CreateIcon(StatusIconCatalog.Warning);

            Assert.That(icon.Width, Is.EqualTo(PlaceholderIconCatalog.IconSize));
            Assert.That(icon.Height, Is.EqualTo(PlaceholderIconCatalog.IconSize));
            Assert.That(CountOpaquePixels(icon), Is.GreaterThan(0));
        }

        [Test]
        public void CreateIcon_UnknownId_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => StatusIconCatalog.CreateIcon("status-unknown"));
        }

        private static int CountOpaquePixels(PixelImage image)
        {
            int count = 0;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    if (image[x, y].Alpha > 0)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
