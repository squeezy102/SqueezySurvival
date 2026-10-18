using NUnit.Framework;
using SqueezySurvival.Core.PixelArt;

namespace SqueezySurvival.Core.Tests.PixelArt
{
    [TestFixture]
    public sealed class PixelFrameImageFactoryTests
    {
        private static readonly PixelColor Outline = new PixelColor(1, 1, 1);
        private static readonly PixelColor Light = new PixelColor(200, 200, 200);
        private static readonly PixelColor Dark = new PixelColor(50, 50, 50);
        private static readonly PixelColor Fill = new PixelColor(100, 100, 100);

        // Outline, one raised ring and a fill: a 2-pixel border around a 2 x 2 center, 6 x 6 in all.
        private static PixelImage Raised() =>
            PixelFrameImageFactory.Create(new PixelFrameStyle(Outline, new[] { new PixelFrameRing(Light, Dark) }, Fill));

        [Test]
        public void Create_IsTheBorderOnEverySideAroundATwoByTwoCenter()
        {
            PixelImage frame = Raised();

            Assert.That((frame.Width, frame.Height), Is.EqualTo((6, 6)));
            Assert.That(frame[2, 2], Is.EqualTo(Fill));
            Assert.That(frame[3, 3], Is.EqualTo(Fill));
        }

        [Test]
        public void Create_OutlineHasSeeThroughCorners()
        {
            PixelImage frame = Raised();

            Assert.That(frame[0, 0].IsVisible, Is.False);
            Assert.That(frame[5, 5].IsVisible, Is.False);
            Assert.That(frame[1, 0], Is.EqualTo(Outline));
            Assert.That(frame[0, 4], Is.EqualTo(Outline));
        }

        [Test]
        public void Create_RingIsLightTopAndLeftAndDarkBottomAndRight()
        {
            PixelImage frame = Raised();

            Assert.That(frame[2, 1], Is.EqualTo(Light));
            Assert.That(frame[1, 3], Is.EqualTo(Light));
            Assert.That(frame[4, 3], Is.EqualTo(Dark));
            Assert.That(frame[2, 4], Is.EqualTo(Dark));
            Assert.That(frame[4, 1], Is.EqualTo(Dark));
        }
    }
}
