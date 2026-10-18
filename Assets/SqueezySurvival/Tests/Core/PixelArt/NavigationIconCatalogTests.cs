using NUnit.Framework;
using SqueezySurvival.Core.PixelArt;

namespace SqueezySurvival.Core.Tests.PixelArt
{
    [TestFixture]
    public sealed class NavigationIconCatalogTests
    {
        [Test]
        public void CreateIcon_UnknownId_IsRefused()
        {
            Assert.That(() => NavigationIconCatalog.CreateIcon("nav-nothing"), Throws.TypeOf<System.ArgumentOutOfRangeException>());
        }
    }
}
