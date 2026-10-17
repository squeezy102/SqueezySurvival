using NUnit.Framework;
using SqueezySurvival.Core.Versioning;

namespace SqueezySurvival.Core.Tests.Versioning
{
    [TestFixture]
    public sealed class GameVersionLabelFormatterTests
    {
        [Test]
        public void Format_InBuild_PrefixesV()
        {
            Assert.That(GameVersionLabelFormatter.Format("0.0.1", false), Is.EqualTo("v0.0.1"));
        }

        [Test]
        public void Format_InEditor_AppendsEditorSuffix()
        {
            Assert.That(GameVersionLabelFormatter.Format("0.0.1", true), Is.EqualTo("v0.0.1 (editor)"));
        }

        [Test]
        public void Format_WithSurroundingWhitespace_TrimsVersion()
        {
            Assert.That(GameVersionLabelFormatter.Format(" 1.2.3 ", false), Is.EqualTo("v1.2.3"));
        }

        [TestCase(null)]
        [TestCase("   ")]
        public void Format_WithoutVersion_Throws(string version)
        {
            Assert.That(() => GameVersionLabelFormatter.Format(version, false), Throws.ArgumentException);
        }
    }
}
