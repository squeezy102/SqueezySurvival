using System;
using NUnit.Framework;
using SqueezySurvival.Editor.Diagnostics;

namespace SqueezySurvival.Editor.Tests.Diagnostics
{
    [TestFixture]
    public sealed class TestRunReportMarkdownFormatterTests
    {
        private static readonly DateTime FinishedAt = new DateTime(2026, 10, 21, 16, 30, 5, DateTimeKind.Utc);

        [Test]
        public void Format_AllPassed_SaysPassedWithCountsAndNoTestSections()
        {
            var report = new TestRunReport(FinishedAt, 1.234, 3, 0, 0, 0, Array.Empty<ITestResultNode>());

            string markdown = TestRunReportMarkdownFormatter.Format(report);

            Assert.That(markdown, Does.Contain("- Result: **Passed**"));
            Assert.That(markdown, Does.Contain("- Finished: 2026-10-21 16:30:05 UTC"));
            Assert.That(markdown, Does.Contain("- Duration: 1.23 s"));
            Assert.That(markdown, Does.Contain("- Total 3: 3 passed, 0 failed, 0 skipped, 0 inconclusive"));
            Assert.That(markdown, Does.Not.Contain("## "));
        }

        [TestCase(1, 0)]
        [TestCase(0, 1)]
        public void Format_AnyFailedOrInconclusive_SaysFailed(int failed, int inconclusive)
        {
            var report = new TestRunReport(FinishedAt, 1, 1, failed, 0, inconclusive, Array.Empty<ITestResultNode>());

            Assert.That(TestRunReportMarkdownFormatter.Format(report), Does.Contain("- Result: **Failed**"));
        }

        [Test]
        public void Format_SkippedOnly_StillSaysPassed()
        {
            var report = new TestRunReport(FinishedAt, 1, 1, 0, 2, 0, Array.Empty<ITestResultNode>());

            Assert.That(TestRunReportMarkdownFormatter.Format(report), Does.Contain("- Result: **Passed**"));
        }

        [Test]
        public void Format_AFailure_ShowsItsNameMessageStackTraceAndOutputFenced()
        {
            var failure = new FakeTestResultNode("Tests.Clock_Shows", TestResultOutcome.Failed, "Expected: 1\n  But was: 2", "at Clock.cs:10", "Log: hello");
            var report = new TestRunReport(FinishedAt, 1, 0, 1, 0, 0, new ITestResultNode[] { failure });

            string markdown = TestRunReportMarkdownFormatter.Format(report);

            Assert.That(markdown, Does.Contain("## Failed: Tests.Clock_Shows"));
            string normalized = markdown.Replace("\r\n", "\n");
            Assert.That(normalized, Does.Contain("Message:\n\n```\nExpected: 1\n  But was: 2\n```"));
            Assert.That(markdown, Does.Contain("at Clock.cs:10"));
            Assert.That(markdown, Does.Contain("Log: hello"));
        }

        [Test]
        public void Format_ASkipWithNoStackTraceOrOutput_LeavesThoseSectionsOut()
        {
            var skipped = new FakeTestResultNode("Tests.Later", TestResultOutcome.Skipped, "Not built yet");
            var report = new TestRunReport(FinishedAt, 1, 0, 0, 1, 0, new ITestResultNode[] { skipped });

            string markdown = TestRunReportMarkdownFormatter.Format(report);

            Assert.That(markdown, Does.Contain("## Skipped: Tests.Later"));
            Assert.That(markdown, Does.Contain("Not built yet"));
            Assert.That(markdown, Does.Not.Contain("Stack trace:"));
            Assert.That(markdown, Does.Not.Contain("Output:"));
        }

        [Test]
        public void FormatRunError_SaysTheRunCouldNotStartAndWhy()
        {
            string markdown = TestRunReportMarkdownFormatter.FormatRunError("Scripts have compile errors.", FinishedAt);

            Assert.That(markdown, Does.Contain("- Result: **Could not run**"));
            Assert.That(markdown, Does.Contain("Scripts have compile errors."));
        }

        [Test]
        public void Format_Null_IsRefused()
        {
            Assert.That(() => TestRunReportMarkdownFormatter.Format(null), Throws.ArgumentNullException);
        }
    }
}
