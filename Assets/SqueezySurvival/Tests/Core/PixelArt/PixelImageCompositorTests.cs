using NUnit.Framework;
using SqueezySurvival.Core.PixelArt;

namespace SqueezySurvival.Core.Tests.PixelArt
{
    [TestFixture]
    public sealed class PixelImageCompositorTests
    {
        [Test]
        public void DrawOver_VisiblePixelsReplaceAndTransparentOnesKeepWhatIsBeneath()
        {
            var sky = new PixelColor(100, 150, 220);
            var palm = new PixelColor(40, 120, 60);
            var target = new PixelImage(2, 1);
            target[0, 0] = sky;
            target[1, 0] = sky;
            var layer = new PixelImage(2, 1);
            layer[1, 0] = palm;

            PixelImageCompositor.DrawOver(target, layer);

            Assert.That(new[] { target[0, 0], target[1, 0] }, Is.EqualTo(new[] { sky, palm }));
        }

        [Test]
        public void DrawOver_DifferentSizes_IsRefused()
        {
            Assert.That(() => PixelImageCompositor.DrawOver(new PixelImage(2, 2), new PixelImage(2, 1)), Throws.ArgumentException);
        }
    }
}
