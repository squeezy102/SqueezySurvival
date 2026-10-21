using System;
using NUnit.Framework;
using SqueezySurvival.Editor.Diagnostics;

namespace SqueezySurvival.Editor.Tests.Diagnostics
{
    [TestFixture]
    public sealed class TestRunReportBuilderTests
    {
        private static readonly DateTime FinishedAt = new DateTime(2026, 10, 21, 16, 30, 0, DateTimeKind.Utc);

        [Test]
        public void Build_CountsOnlyLeafTestsAcrossNestedSuites()
        {
            var root = new FakeTestResultNode(
                "Run",
                2.5,
                new FakeTestResultNode(
                    "Fixture",
                    1.0,
                    new FakeTestResultNode("Fixture.A", TestResultOutcome.Passed),
                    new FakeTestResultNode("Fixture.B", TestResultOutcome.Failed)),
                new FakeTestResultNode(
                    "Fixture.Cases",
                    1.0,
                    new FakeTestResultNode("Fixture.Cases(1)", TestResultOutcome.Passed),
                    new FakeTestResultNode("Fixture.Cases(2)", TestResultOutcome.Skipped),
                    new FakeTestResultNode("Fixture.Cases(3)", TestResultOutcome.Inconclusive)));

            TestRunReport report = TestRunReportBuilder.Build(root, FinishedAt);

            Assert.That(report.Passed, Is.EqualTo(2));
            Assert.That(report.Failed, Is.EqualTo(1));
            Assert.That(report.Skipped, Is.EqualTo(1));
            Assert.That(report.Inconclusive, Is.EqualTo(1));
            Assert.That(report.Total, Is.EqualTo(5));
            Assert.That(report.DurationSeconds, Is.EqualTo(2.5));
            Assert.That(report.FinishedAtUtc, Is.EqualTo(FinishedAt));
        }

        [Test]
        public void Build_ListsEveryTestThatDidNotPassInRunOrder()
        {
            var root = new FakeTestResultNode(
                "Run",
                1.0,
                new FakeTestResultNode("A", TestResultOutcome.Skipped),
                new FakeTestResultNode("B", TestResultOutcome.Passed),
                new FakeTestResultNode("C", TestResultOutcome.Failed));

            TestRunReport report = TestRunReportBuilder.Build(root, FinishedAt);

            Assert.That(report.TestsNotPassed, Has.Count.EqualTo(2));
            Assert.That(report.TestsNotPassed[0].FullName, Is.EqualTo("A"));
            Assert.That(report.TestsNotPassed[1].FullName, Is.EqualTo("C"));
        }

        [Test]
        public void Build_ARunOfOneTest_CountsThatTest()
        {
            TestRunReport report = TestRunReportBuilder.Build(new FakeTestResultNode("Only", TestResultOutcome.Failed), FinishedAt);

            Assert.That(report.Failed, Is.EqualTo(1));
            Assert.That(report.Total, Is.EqualTo(1));
        }

        [Test]
        public void Build_Null_IsRefused()
        {
            Assert.That(() => TestRunReportBuilder.Build(null, FinishedAt), Throws.ArgumentNullException);
        }
    }
}
