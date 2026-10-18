using System;
using NUnit.Framework;
using SqueezySurvival.Core.Runs;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class PointEarningRateTests
    {
        [Test]
        public void Constructor_WithNegativePoints_Throws()
        {
            Assert.That(() => new PointEarningRate(-1, 1), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Constructor_WithZeroDays_Throws()
        {
            Assert.That(() => new PointEarningRate(1, 0), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
