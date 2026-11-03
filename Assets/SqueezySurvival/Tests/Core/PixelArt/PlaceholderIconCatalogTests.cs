using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.PixelArt;

namespace SqueezySurvival.Core.Tests.PixelArt
{
    [TestFixture]
    public sealed class PlaceholderIconCatalogTests
    {
        [Test]
        public void UnknownId_HasNoIcon()
        {
            Assert.That(PlaceholderIconCatalog.HasIcon("not-a-real-thing"), Is.False);
        }

        [Test]
        public void CreateIcon_IsSixteenPixelsWithAClearMarginOutsideTheOutline()
        {
            PixelImage icon = PlaceholderIconCatalog.CreateIcon("stone");

            Assert.That((icon.Width, icon.Height), Is.EqualTo((16, 16)));
            Assert.That(Enumerable.Range(0, 16).Any(x => Enumerable.Range(0, 16).Any(y => icon[x, y].IsVisible)), Is.True);
        }
    }
}
