using NUnit.Framework;
using SqueezySurvival.Core.Options;

namespace SqueezySurvival.Core.Tests.Options
{
    [TestFixture]
    public sealed class GridPanelScrollWheelMapperTests
    {
        [Test]
        public void WheelDown_NotInverted_ScrollsTowardEnd()
        {
            Assert.That(GridPanelScrollWheelMapper.ScrollOffsetChange(GridPanelLayoutOptions.Default, 0f, 1f, 20f), Is.EqualTo(20f));
        }

        [Test]
        public void WheelDown_Inverted_ScrollsTowardStart()
        {
            GridPanelLayoutOptions inverted = GridPanelLayoutOptions.Default.WithInvertScrollWheel(true);

            Assert.That(GridPanelScrollWheelMapper.ScrollOffsetChange(inverted, 0f, 1f, 20f), Is.EqualTo(-20f));
        }

        [Test]
        public void SidewaysOnly_UsesSidewaysMovement()
        {
            Assert.That(GridPanelScrollWheelMapper.ScrollOffsetChange(GridPanelLayoutOptions.Default, -2f, 0f, 10f), Is.EqualTo(-20f));
        }

        [Test]
        public void Diagonal_UsesOrdinaryWheelOnly()
        {
            Assert.That(GridPanelScrollWheelMapper.ScrollOffsetChange(GridPanelLayoutOptions.Default, 5f, 1f, 10f), Is.EqualTo(10f));
        }
    }
}
