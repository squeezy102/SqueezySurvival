using NUnit.Framework;
using SqueezySurvival.Core.EventLog;

namespace SqueezySurvival.Core.Tests.EventLog
{
    [TestFixture]
    public sealed class UnreadWarningIndicatorTextFormatterTests
    {
        [TestCase(-1, "")]
        [TestCase(0, "")]
        [TestCase(1, "!")]
        [TestCase(2, "! 2")]
        [TestCase(99, "! 99")]
        public void Format_UnreadCount_ShowsTheMarkAndANumberOnlyForSeveral(int unreadCount, string expected)
        {
            Assert.That(UnreadWarningIndicatorTextFormatter.Format(unreadCount), Is.EqualTo(expected));
        }
    }
}
