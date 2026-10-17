using NUnit.Framework;
using SqueezySurvival.Core.Options;

namespace SqueezySurvival.Core.Tests.Options
{
    [TestFixture]
    public sealed class PlayerOptionsTests
    {
        [Test]
        public void SetGridPanelLayout_ChangesOnlyThatPanelAndRaisesChanged()
        {
            var options = new PlayerOptions();
            GridPanel? changed = null;
            options.GridPanelLayoutChanged += panel => changed = panel;
            GridPanelLayoutOptions horizontal = GridPanelLayoutOptions.Default.WithScrollDirection(GridPanelScrollDirection.Horizontal);

            options.SetGridPanelLayout(GridPanel.GroundPool, horizontal);

            Assert.That(changed, Is.EqualTo(GridPanel.GroundPool));
            Assert.That(options.GridPanelLayout(GridPanel.GroundPool), Is.SameAs(horizontal));
            Assert.That(options.GridPanelLayout(GridPanel.ZoneFeatures), Is.SameAs(GridPanelLayoutOptions.Default));
        }

        [Test]
        public void SetGridPanelLayout_SameValues_DoesNotRaiseChanged()
        {
            var options = new PlayerOptions();
            int changes = 0;
            options.GridPanelLayoutChanged += _ => changes++;

            options.SetGridPanelLayout(GridPanel.ZoneFeatures, GridPanelLayoutOptions.Default.WithInvertScrollWheel(false));

            Assert.That(changes, Is.Zero);
        }
    }
}
