using System;
using System.Collections.Generic;

namespace SqueezySurvival.Editor.Diagnostics
{
    /// <summary>What a test run produced: counts of each outcome and every test that did not pass. Immutable.</summary>
    public sealed class TestRunReport
    {
        /// <summary>Describes a finished run.</summary>
        public TestRunReport(
            DateTime finishedAtUtc,
            double durationSeconds,
            int passed,
            int failed,
            int skipped,
            int inconclusive,
            IReadOnlyList<ITestResultNode> testsNotPassed)
        {
            FinishedAtUtc = finishedAtUtc;
            DurationSeconds = durationSeconds;
            Passed = passed;
            Failed = failed;
            Skipped = skipped;
            Inconclusive = inconclusive;
            TestsNotPassed = testsNotPassed ?? throw new ArgumentNullException(nameof(testsNotPassed));
        }

        /// <summary>When the run finished, in UTC.</summary>
        public DateTime FinishedAtUtc { get; }

        /// <summary>How long the run took, in seconds.</summary>
        public double DurationSeconds { get; }

        /// <summary>Tests that passed.</summary>
        public int Passed { get; }

        /// <summary>Tests that failed.</summary>
        public int Failed { get; }

        /// <summary>Tests that were skipped or ignored.</summary>
        public int Skipped { get; }

        /// <summary>Tests that ended inconclusive.</summary>
        public int Inconclusive { get; }

        /// <summary>Every test that did not pass, in run order.</summary>
        public IReadOnlyList<ITestResultNode> TestsNotPassed { get; }

        /// <summary>Total tests run.</summary>
        public int Total => Passed + Failed + Skipped + Inconclusive;
    }
}
