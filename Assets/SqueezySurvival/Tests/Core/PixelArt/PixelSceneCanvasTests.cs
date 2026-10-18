using NUnit.Framework;
using SqueezySurvival.Core.PixelArt;

namespace SqueezySurvival.Core.Tests.PixelArt
{
    [TestFixture]
    public sealed class PixelSceneCanvasTests
    {
        private static readonly PixelColor Light = new PixelColor(200, 200, 200);
        private static readonly PixelColor Dark = new PixelColor(20, 20, 20);

        // Two bands over 8 rows: row 0 is all the first band, and the dither turns over as the rows go down.
        private static PixelImage TwoBands()
        {
            var image = new PixelImage(4, 8);
            new PixelSceneCanvas(image).DitheredBands(0, 8, new[] { Light, Dark });
            return image;
        }

        [Test]
        public void DitheredBands_FirstRow_IsTheFirstBand()
        {
            PixelImage image = TwoBands();

            Assert.That(new[] { image[0, 0], image[1, 0], image[2, 0], image[3, 0] }, Is.All.EqualTo(Light));
        }

        [Test]
        public void DitheredBands_HalfwayRows_MixBothBandsInAChecker()
        {
            // Rows 4 and 5 are half to five-eighths of the way: thresholds 0.125 and 0.375 turn over, 0.625 and 0.875 don't.
            PixelImage image = TwoBands();

            Assert.That(new[] { image[0, 4], image[1, 4], image[0, 5], image[1, 5] }, Is.EqualTo(new[] { Dark, Light, Light, Dark }));
        }

        [Test]
        public void DitheredBands_RowsOutsideTheImage_AreRefused()
        {
            var canvas = new PixelSceneCanvas(new PixelImage(4, 8));

            Assert.That(() => canvas.DitheredBands(0, 9, new[] { Light, Dark }), Throws.TypeOf<System.ArgumentOutOfRangeException>());
        }
    }
}
