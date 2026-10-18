using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Navigation;
using SqueezySurvival.Core.PixelArt;

namespace SqueezySurvival.Core.Tests.Navigation
{
    [TestFixture]
    public sealed class NavigationBarEntriesTests
    {
        [Test]
        public void EveryEntry_HasAnIconTheCatalogCanDraw()
        {
            var entries = NavigationBarEntries.GameplayWindows.Append(NavigationBarEntries.Options);

            Assert.That(() => entries.Select(entry => NavigationIconCatalog.CreateIcon(entry.IconId)).ToList(), Throws.Nothing);
        }

        [Test]
        public void AvailabilityNote_SaysOnlyUnbuiltWindowsAreNotBuilt()
        {
            Assert.That(NavigationBarEntries.Options.AvailabilityNote, Is.Empty);
            Assert.That(NavigationBarEntries.GameplayWindows.Single(entry => entry.Key == "map").AvailabilityNote, Is.EqualTo("Not built yet."));
        }

    }
}
