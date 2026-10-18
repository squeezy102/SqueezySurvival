using System;
using NUnit.Framework;
using SqueezySurvival.Core.Randomness;

namespace SqueezySurvival.Core.Tests.Randomness
{
    [TestFixture]
    public sealed class SystemRandomNumberSourceTests
    {
        [TestCase(5, 5)]
        [TestCase(5, 2)]
        public void NextInt_WithEmptyRange_Throws(int minimum, int maximum)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SystemRandomNumberSource(1).NextInt(minimum, maximum));
        }
    }
}
