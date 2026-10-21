using NUnit.Framework;
using SqueezySurvival.Core.Navigation;

namespace SqueezySurvival.Core.Tests.Navigation
{
    [TestFixture]
    public sealed class NotBuiltNoticeTextFormatterTests
    {
        [Test]
        public void Format_NamesTheWindow()
        {
            var entry = new NavigationBarEntry("crafting", "Crafting", "Make things.", "crafting", isBuilt: false);

            Assert.That(NotBuiltNoticeTextFormatter.Format(entry), Is.EqualTo("Crafting isn't built yet."));
        }

        [Test]
        public void Format_Null_IsRefused()
        {
            Assert.That(() => NotBuiltNoticeTextFormatter.Format(null), Throws.ArgumentNullException);
        }
    }
}
