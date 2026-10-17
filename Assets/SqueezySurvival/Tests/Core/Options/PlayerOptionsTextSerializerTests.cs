using NUnit.Framework;
using SqueezySurvival.Core.Options;

namespace SqueezySurvival.Core.Tests.Options
{
    [TestFixture]
    public sealed class PlayerOptionsTextSerializerTests
    {
        [Test]
        public void Serialize_WritesVersionAndEveryPanel()
        {
            string text = PlayerOptionsTextSerializer.Serialize(new PlayerOptions());

            StringAssert.StartsWith("format-version=1\n", text);
            StringAssert.Contains("grid-panel.ground-pool.scroll-direction=Vertical\n", text);
            StringAssert.Contains("grid-panel.zone-features.columns=6\n", text);
        }

        [Test]
        public void Deserialize_InvalidPanel_KeepsItsDefaultsAndReadsOthers()
        {
            const string Text =
                "grid-panel.zone-features.scroll-direction=Sideways\n" +
                "grid-panel.zone-features.columns=6\n" +
                "grid-panel.zone-features.rows=3\n" +
                "grid-panel.zone-features.invert-scroll-wheel=false\n" +
                "grid-panel.ground-pool.scroll-direction=Horizontal\n" +
                "grid-panel.ground-pool.columns=5\n" +
                "grid-panel.ground-pool.rows=2\n" +
                "grid-panel.ground-pool.invert-scroll-wheel=true\n" +
                "some-future-setting=whatever\n";

            PlayerOptions options = PlayerOptionsTextSerializer.Deserialize(Text);

            Assert.That(options.GridPanelLayout(GridPanel.ZoneFeatures), Is.SameAs(GridPanelLayoutOptions.Default));
            Assert.That(
                options.GridPanelLayout(GridPanel.GroundPool).HasSameValuesAs(new GridPanelLayoutOptions(GridPanelScrollDirection.Horizontal, 5, 2, true)),
                Is.True);
        }

        [Test]
        public void Deserialize_CountOutsideLimits_KeepsDefaults()
        {
            const string Text =
                "grid-panel.ground-pool.scroll-direction=Vertical\n" +
                "grid-panel.ground-pool.columns=40\n" +
                "grid-panel.ground-pool.rows=3\n" +
                "grid-panel.ground-pool.invert-scroll-wheel=false\n";

            Assert.That(PlayerOptionsTextSerializer.Deserialize(Text).GridPanelLayout(GridPanel.GroundPool), Is.SameAs(GridPanelLayoutOptions.Default));
        }

        [Test]
        public void Deserialize_EmptyText_GivesDefaults()
        {
            Assert.That(PlayerOptionsTextSerializer.Deserialize(string.Empty).GridPanelLayout(GridPanel.GroundPool), Is.SameAs(GridPanelLayoutOptions.Default));
        }
    }
}
