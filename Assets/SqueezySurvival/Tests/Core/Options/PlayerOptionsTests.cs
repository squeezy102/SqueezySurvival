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

        [Test]
        public void CopyGridPanelLayoutsFrom_CopiesOnlyTheGivenPanels()
        {
            var run = new PlayerOptions();
            run.SetGridPanelLayout(GridPanel.ZoneFeatures, GridPanelLayoutOptions.Default.WithInvertScrollWheel(true));
            run.SetGridPanelLayout(GridPanel.GroundPool, GridPanelLayoutOptions.Default.WithFixedLineCount(8));
            var preference = new PlayerOptions();

            preference.CopyGridPanelLayoutsFrom(run, new[] { GridPanel.GroundPool });

            Assert.That(preference.GridPanelLayout(GridPanel.GroundPool).ColumnCount, Is.EqualTo(8));
            Assert.That(preference.GridPanelLayout(GridPanel.ZoneFeatures).InvertScrollWheel, Is.False);
        }

        [Test]
        public void ResetGridPanelLayouts_RestoresDefaultsForTheGivenPanels()
        {
            var options = new PlayerOptions();
            options.SetGridPanelLayout(GridPanel.ZoneFeatures, GridPanelLayoutOptions.Default.WithFixedLineCount(4));
            options.SetGridPanelLayout(GridPanel.GroundPool, GridPanelLayoutOptions.Default.WithFixedLineCount(4));

            options.ResetGridPanelLayouts(new[] { GridPanel.ZoneFeatures });

            Assert.That(options.GridPanelLayout(GridPanel.ZoneFeatures), Is.SameAs(GridPanelLayoutOptions.Default));
            Assert.That(options.GridPanelLayout(GridPanel.GroundPool).ColumnCount, Is.EqualTo(4));
        }

        [Test]
        public void Copy_IsIndependentOfTheOriginal()
        {
            var original = new PlayerOptions();
            PlayerOptions copy = original.Copy();

            copy.SetGridPanelLayout(GridPanel.GroundPool, GridPanelLayoutOptions.Default.WithInvertScrollWheel(true));

            Assert.That(original.GridPanelLayout(GridPanel.GroundPool).InvertScrollWheel, Is.False);
        }
    }
}
