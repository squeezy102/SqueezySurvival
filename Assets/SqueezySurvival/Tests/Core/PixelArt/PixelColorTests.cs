using System;
using NUnit.Framework;
using SqueezySurvival.Core.PixelArt;

namespace SqueezySurvival.Core.Tests.PixelArt
{
    [TestFixture]
    public sealed class PixelColorTests
    {
        private static readonly PixelColor Tan = new PixelColor(200, 100, 0, 128);

        [Test]
        public void Lightened_MovesEachChannelTowardWhiteAndKeepsOpacity()
        {
            Assert.That(Tan.Lightened(0.5), Is.EqualTo(new PixelColor(228, 178, 128, 128)));
        }

        [Test]
        public void Darkened_MovesEachChannelTowardBlackAndKeepsOpacity()
        {
            Assert.That(Tan.Darkened(0.25), Is.EqualTo(new PixelColor(150, 75, 0, 128)));
        }

        [TestCase(0.0, true)]
        [TestCase(1.0, true)]
        [TestCase(-0.01, false)]
        [TestCase(1.01, false)]
        public void Darkened_AmountMustBeFromZeroToOne(double amount, bool allowed)
        {
            TestDelegate darken = () => Tan.Darkened(amount);

            if (allowed)
            {
                Assert.That(darken, Throws.Nothing);
            }
            else
            {
                Assert.That(darken, Throws.TypeOf<ArgumentOutOfRangeException>());
            }
        }
    }
}
