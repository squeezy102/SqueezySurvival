using System.Collections.Generic;
using SqueezySurvival.Editor.Diagnostics;

namespace SqueezySurvival.Editor.Tests.Diagnostics
{
    /// <summary>A result node built in a test, standing in for the Test Framework's result.</summary>
    public sealed class FakeTestResultNode : ITestResultNode
    {
        /// <summary>A single test with the given outcome.</summary>
        public FakeTestResultNode(string fullName, TestResultOutcome outcome, string message = "", string stackTrace = "", string output = "")
        {
            FullName = fullName;
            Outcome = outcome;
            Message = message;
            StackTrace = stackTrace;
            Output = output;
            Children = new List<ITestResultNode>();
        }

        /// <summary>A suite holding <paramref name="children"/>; its own outcome is <see cref="TestResultOutcome.Failed"/> so tests prove it is not counted.</summary>
        public FakeTestResultNode(string fullName, double durationSeconds, params ITestResultNode[] children)
        {
            FullName = fullName;
            Outcome = TestResultOutcome.Failed;
            Message = string.Empty;
            StackTrace = string.Empty;
            Output = string.Empty;
            DurationSeconds = durationSeconds;
            Children = children;
        }

        /// <inheritdoc />
        public string FullName { get; }

        /// <inheritdoc />
        public TestResultOutcome Outcome { get; }

        /// <inheritdoc />
        public string Message { get; }

        /// <inheritdoc />
        public string StackTrace { get; }

        /// <inheritdoc />
        public string Output { get; }

        /// <inheritdoc />
        public double DurationSeconds { get; }

        /// <inheritdoc />
        public IReadOnlyList<ITestResultNode> Children { get; }
    }
}
