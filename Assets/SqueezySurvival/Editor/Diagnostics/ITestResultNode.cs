using System.Collections.Generic;

namespace SqueezySurvival.Editor.Diagnostics
{
    /// <summary>
    /// One node of a test run's result tree, as the report needs it: an assembly, fixture, test or test case. Wraps the
    /// Test Framework's result so the report's logic can be tested without running tests.
    /// </summary>
    public interface ITestResultNode
    {
        /// <summary>The node's full name, such as <c>SqueezySurvival.Core.Tests.Time.GameClockTests.RunStart_IsDayOneAtSixAm</c>.</summary>
        string FullName { get; }

        /// <summary>How the node ended.</summary>
        TestResultOutcome Outcome { get; }

        /// <summary>The failure or skip message; empty when there is none.</summary>
        string Message { get; }

        /// <summary>Where a failure happened; empty when there is none.</summary>
        string StackTrace { get; }

        /// <summary>What the test wrote while it ran, including Unity log messages; empty when nothing.</summary>
        string Output { get; }

        /// <summary>How long the node took, in seconds.</summary>
        double DurationSeconds { get; }

        /// <summary>The nodes inside this one. Empty for a single test or test case.</summary>
        IReadOnlyList<ITestResultNode> Children { get; }
    }
}
