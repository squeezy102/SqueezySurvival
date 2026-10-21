using NUnit.Framework;
using SqueezySurvival.Core.EventLog;

namespace SqueezySurvival.Core.Tests.EventLog
{
    [TestFixture]
    public sealed class ScrolledIntoViewCalculatorTests
    {
        private const float ViewTop = 100f;
        private const float ViewBottom = 300f;

        [TestCase(100f, 300f, true)]
        [TestCase(150f, 200f, true)]
        [TestCase(99.5f, 300.5f, true)]
        [TestCase(99.4f, 200f, false)]
        [TestCase(200f, 300.6f, false)]
        [TestCase(50f, 150f, false)]
        [TestCase(310f, 340f, false)]
        public void IsFullyInView_EdgesAgainstTheViewWithinHalfAPixel(float top, float bottom, bool expected)
        {
            Assert.That(ScrolledIntoViewCalculator.IsFullyInView(top, bottom, ViewTop, ViewBottom), Is.EqualTo(expected));
        }

        [TestCase(500f, 500f, true)]
        [TestCase(499.5f, 500f, true)]
        [TestCase(499.4f, 500f, false)]
        [TestCase(0f, 500f, false)]
        public void IsScrolledToEnd_WithinHalfAPixelOfTheEnd(float position, float end, bool expected)
        {
            Assert.That(ScrolledIntoViewCalculator.IsScrolledToEnd(position, end), Is.EqualTo(expected));
        }
    }
}
