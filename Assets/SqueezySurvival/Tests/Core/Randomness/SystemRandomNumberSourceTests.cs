using NUnit.Framework;
using SqueezySurvival.Core.Randomness;

namespace SqueezySurvival.Core.Tests.Randomness
{
    [TestFixture]
    public sealed class SystemRandomNumberSourceTests
    {
        [Test]
        public void NextInt_SameSeed_GivesSameSequence()
        {
            var first = new SystemRandomNumberSource(42);
            var second = new SystemRandomNumberSource(42);

            for (int index = 0; index < 10; index++)
            {
                Assert.That(first.NextInt(0, 1000), Is.EqualTo(second.NextInt(0, 1000)));
            }
        }

        [Test]
        public void NextInt_StaysInsideRange()
        {
            var source = new SystemRandomNumberSource(7);

            for (int index = 0; index < 100; index++)
            {
                Assert.That(source.NextInt(3, 6), Is.InRange(3, 5));
            }
        }
    }
}
