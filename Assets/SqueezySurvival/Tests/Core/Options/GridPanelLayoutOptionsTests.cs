using System;
using NUnit.Framework;
using SqueezySurvival.Core.Options;

namespace SqueezySurvival.Core.Tests.Options
{
    [TestFixture]
    public sealed class GridPanelLayoutOptionsTests
    {
        [Test]
        public void Default_IsVerticalSixColumnsNotInverted()
        {
            GridPanelLayoutOptions layout = GridPanelLayoutOptions.Default;

            Assert.That(layout.ScrollDirection, Is.EqualTo(GridPanelScrollDirection.Vertical));
            Assert.That(layout.FixedLineCount, Is.EqualTo(6));
            Assert.That(layout.InvertScrollWheel, Is.False);
        }

        [TestCase(3, 3)]
        [TestCase(9, 3)]
        [TestCase(6, 1)]
        [TestCase(6, 5)]
        public void Constructor_CountOutsideLimits_Throws(int columns, int rows)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new GridPanelLayoutOptions(GridPanelScrollDirection.Vertical, columns, rows, false));
        }

        [Test]
        public void WithScrollDirection_Horizontal_UsesRowsAsFixedCount()
        {
            GridPanelLayoutOptions horizontal = GridPanelLayoutOptions.Default.WithScrollDirection(GridPanelScrollDirection.Horizontal);

            Assert.That(horizontal.FixedLineCount, Is.EqualTo(3));
            Assert.That(horizontal.MaximumFixedLineCount, Is.EqualTo(4));
        }

        [Test]
        public void SwitchingDirectionBack_RestoresEarlierCount()
        {
            GridPanelLayoutOptions layout = GridPanelLayoutOptions.Default
                .WithFixedLineCount(8)
                .WithScrollDirection(GridPanelScrollDirection.Horizontal)
                .WithFixedLineCount(2)
                .WithScrollDirection(GridPanelScrollDirection.Vertical);

            Assert.That(layout.FixedLineCount, Is.EqualTo(8));
            Assert.That(layout.RowCount, Is.EqualTo(2));
        }

        [TestCase(20, 8)]
        [TestCase(0, 4)]
        public void WithFixedLineCount_ClampsToLimits(int requested, int expected)
        {
            Assert.That(GridPanelLayoutOptions.Default.WithFixedLineCount(requested).ColumnCount, Is.EqualTo(expected));
        }
    }
}
