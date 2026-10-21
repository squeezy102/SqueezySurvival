using System;
using System.Collections.Generic;

namespace SqueezySurvival.Editor.Diagnostics
{
    /// <summary>Builds a <see cref="TestRunReport"/> from a run's result tree, counting only single tests and test cases.</summary>
    public static class TestRunReportBuilder
    {
        /// <summary>Walks <paramref name="root"/> and reports each leaf test's outcome.</summary>
        public static TestRunReport Build(ITestResultNode root, DateTime finishedAtUtc)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var counts = new Dictionary<TestResultOutcome, int>
            {
                [TestResultOutcome.Passed] = 0,
                [TestResultOutcome.Failed] = 0,
                [TestResultOutcome.Skipped] = 0,
                [TestResultOutcome.Inconclusive] = 0,
            };
            var notPassed = new List<ITestResultNode>();
            CountLeaves(root, counts, notPassed);
            return new TestRunReport(
                finishedAtUtc,
                root.DurationSeconds,
                counts[TestResultOutcome.Passed],
                counts[TestResultOutcome.Failed],
                counts[TestResultOutcome.Skipped],
                counts[TestResultOutcome.Inconclusive],
                notPassed);
        }

        // A node with children is a suite (assembly, namespace, fixture or parameterized test); only leaves are tests.
        private static void CountLeaves(ITestResultNode node, Dictionary<TestResultOutcome, int> counts, List<ITestResultNode> notPassed)
        {
            if (node.Children.Count > 0)
            {
                foreach (ITestResultNode child in node.Children)
                {
                    CountLeaves(child, counts, notPassed);
                }

                return;
            }

            counts[node.Outcome]++;
            if (node.Outcome != TestResultOutcome.Passed)
            {
                notPassed.Add(node);
            }
        }
    }
}
