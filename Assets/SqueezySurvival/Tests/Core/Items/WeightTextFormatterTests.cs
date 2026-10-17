using NUnit.Framework;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Items
{
    [TestFixture]
    public sealed class WeightTextFormatterTests
    {
        [TestCase(1.0, "1 lb")]
        [TestCase(2.25, "2.25 lb")]
        [TestCase(0.125, "0.13 lb")]
        public void FormatPounds_WritesAtMostTwoDecimals(double pounds, string expected)
        {
            Assert.That(WeightTextFormatter.FormatPounds((decimal)pounds), Is.EqualTo(expected));
        }
    }
}
