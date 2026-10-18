using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.PixelArt;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.PixelArt
{
    [TestFixture]
    public sealed class PlaceholderIconCatalogTests
    {
        [Test]
        public void EveryBeachFindAndFeature_HasItsOwnIcon()
        {
            ZoneDefinition beach = PlaceholderBeachZoneContent.Beach;
            var ids = beach.FindPool.Entries.Select(entry => entry.Outcome.Id)
                .Concat(beach.FeaturePool.Entries.Select(entry => entry.Outcome.Id));

            Assert.That(ids.Where(id => !PlaceholderIconCatalog.HasIcon(id)), Is.Empty);
        }

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
