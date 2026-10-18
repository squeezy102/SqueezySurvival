using NUnit.Framework;
using SqueezySurvival.Core.PixelArt;

namespace SqueezySurvival.Core.Tests.PixelArt
{
    [TestFixture]
    public sealed class PixelIconStylizerTests
    {
        private static readonly PixelColor Green = new PixelColor(100, 200, 100);
        private static readonly PixelColor Gray = new PixelColor(120, 120, 120);

        // A 3 x 3 green block in the middle of a 5 x 5 image.
        private static PixelImage GreenBlock()
        {
            var image = new PixelImage(5, 5);
            for (int y = 1; y <= 3; y++)
            {
                for (int x = 1; x <= 3; x++)
                {
                    image[x, y] = Green;
                }
            }

            return image;
        }

        [Test]
        public void Stylize_LightsTopLeftEdgesShadowsBottomRightAndLeavesTheMiddle()
        {
            PixelImage styled = PixelIconStylizer.Stylize(GreenBlock());

            Assert.That(styled[1, 1], Is.EqualTo(Green.Lightened(PixelIconStylizer.HighlightAmount)));
            Assert.That(styled[2, 2], Is.EqualTo(Green));
            Assert.That(styled[3, 2], Is.EqualTo(Green.Darkened(PixelIconStylizer.ShadowAmount)));
            Assert.That(styled[2, 3], Is.EqualTo(Green.Darkened(PixelIconStylizer.ShadowAmount)));
        }

        [Test]
        public void Stylize_PixelBothLitAndShadowed_IsShadowed()
        {
            PixelImage styled = PixelIconStylizer.Stylize(GreenBlock());

            Assert.That(styled[3, 1], Is.EqualTo(Green.Darkened(PixelIconStylizer.ShadowAmount)));
        }

        [Test]
        public void Stylize_OutlinesEmptyPixelsBesideTheShapeButNotDiagonally()
        {
            PixelImage styled = PixelIconStylizer.Stylize(GreenBlock());

            Assert.That(styled[0, 2], Is.EqualTo(Green.Darkened(PixelIconStylizer.OutlineAmount)));
            Assert.That(styled[2, 4], Is.EqualTo(Green.Darkened(PixelIconStylizer.OutlineAmount)));
            Assert.That(styled[0, 0].IsVisible, Is.False);
        }

        [Test]
        public void Stylize_OutlineTakesTheDarkestNeighbor()
        {
            PixelImage image = GreenBlock();
            image[2, 1] = Gray;

            PixelImage styled = PixelIconStylizer.Stylize(image);

            Assert.That(styled[2, 0], Is.EqualTo(Gray.Darkened(PixelIconStylizer.OutlineAmount)));
        }
    }
}
