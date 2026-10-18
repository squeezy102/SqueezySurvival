using NUnit.Framework;
using SqueezySurvival.Core.PixelArt;

namespace SqueezySurvival.Core.Tests.PixelArt
{
    [TestFixture]
    public sealed class DayNightDialPainterTests
    {
        [TestCase(0, -180.0)]
        [TestCase(360, -90.0)]
        [TestCase(720, 0.0)]
        [TestCase(1080, 90.0)]
        [TestCase(1439, 179.75)]
        public void RotationDegrees_TurnsOnceADayWithNoonAtTheTop(int minuteOfDay, double expected)
        {
            Assert.That(DayNightDialPainter.RotationDegrees(minuteOfDay), Is.EqualTo(expected).Within(0.001));
        }

        [TestCase(-1)]
        [TestCase(1440)]
        public void RotationDegrees_OutsideTheDay_IsRefused(int minuteOfDay)
        {
            Assert.That(() => DayNightDialPainter.RotationDegrees(minuteOfDay), Throws.TypeOf<System.ArgumentOutOfRangeException>());
        }

        [Test]
        public void Paint_TopOfTheDial_IsBrightAtNoonAndDimAtMidnight()
        {
            // Straight up from the center, where the sun sits at noon.
            int x = DayNightDialPainter.Radius;
            const int y = 9;

            PixelColor noon = DayNightDialPainter.Paint(720)[x, y];
            PixelColor midnight = DayNightDialPainter.Paint(0)[x, y];

            Assert.That(noon.Luma, Is.GreaterThan(200));
            Assert.That(midnight.Luma, Is.LessThan(100));
        }
    }
}
