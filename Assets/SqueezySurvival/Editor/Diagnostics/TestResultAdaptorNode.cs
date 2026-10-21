using System;
using System.Collections.Generic;
using UnityEditor.TestTools.TestRunner.Api;

namespace SqueezySurvival.Editor.Diagnostics
{
    /// <summary>Shows the Test Framework's <see cref="ITestResultAdaptor"/> as an <see cref="ITestResultNode"/>. Forwards only.</summary>
    public sealed class TestResultAdaptorNode : ITestResultNode
    {
        private readonly ITestResultAdaptor _result;
        private IReadOnlyList<ITestResultNode> _children;

        /// <summary>Wraps <paramref name="result"/>.</summary>
        public TestResultAdaptorNode(ITestResultAdaptor result)
        {
            _result = result ?? throw new ArgumentNullException(nameof(result));
        }

        /// <inheritdoc />
        public string FullName => _result.FullName;

        /// <inheritdoc />
        public TestResultOutcome Outcome => ToOutcome(_result.TestStatus);

        /// <inheritdoc />
        public string Message => _result.Message ?? string.Empty;

        /// <inheritdoc />
        public string StackTrace => _result.StackTrace ?? string.Empty;

        /// <inheritdoc />
        public string Output => _result.Output ?? string.Empty;

        /// <inheritdoc />
        public double DurationSeconds => _result.Duration;

        /// <inheritdoc />
        public IReadOnlyList<ITestResultNode> Children => _children ??= WrapChildren();

        private static TestResultOutcome ToOutcome(TestStatus status)
        {
            switch (status)
            {
                case TestStatus.Passed:
                    return TestResultOutcome.Passed;
                case TestStatus.Failed:
                    return TestResultOutcome.Failed;
                case TestStatus.Skipped:
                    return TestResultOutcome.Skipped;
                case TestStatus.Inconclusive:
                    return TestResultOutcome.Inconclusive;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown test status.");
            }
        }

        private IReadOnlyList<ITestResultNode> WrapChildren()
        {
            var children = new List<ITestResultNode>();
            if (_result.HasChildren)
            {
                foreach (ITestResultAdaptor child in _result.Children)
                {
                    children.Add(new TestResultAdaptorNode(child));
                }
            }

            return children;
        }
    }
}
