using System;
using System.IO;
using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;

namespace SqueezySurvival.Editor.Diagnostics
{
    /// <summary>
    /// After every test run, including runs from the Test Runner window, writes the results to the <see cref="DiagnosticsFolder"/>
    /// so they can be read without opening Unity (Docs/Engineering/TestingStandards.md, Reading results without Unity). Registered on
    /// every script reload, because the Test Framework does not keep callbacks across domain reloads
    /// (Unity Manual: Test Framework, retrieve test results).
    /// </summary>
    [InitializeOnLoad]
    public sealed class TestRunResultsExporter : IErrorCallbacks
    {
        /// <summary>The last run's full results, in NUnit XML.</summary>
        public const string ResultsXmlPath = DiagnosticsFolder.Path + "/latest-test-results.xml";

        /// <summary>The last run's summary in Markdown: counts, then every test that did not pass.</summary>
        public const string SummaryMarkdownPath = DiagnosticsFolder.Path + "/latest-test-summary.md";

        static TestRunResultsExporter()
        {
            TestRunnerApi.RegisterTestCallback(new TestRunResultsExporter());
        }

        /// <inheritdoc />
        public void RunStarted(ITestAdaptor testsToRun)
        {
        }

        /// <inheritdoc />
        public void RunFinished(ITestResultAdaptor result)
        {
            Directory.CreateDirectory(DiagnosticsFolder.Path);
            TestRunnerApi.SaveResultToFile(result, ResultsXmlPath);
            TestRunReport report = TestRunReportBuilder.Build(new TestResultAdaptorNode(result), DateTime.UtcNow);
            File.WriteAllText(SummaryMarkdownPath, TestRunReportMarkdownFormatter.Format(report));
        }

        /// <inheritdoc />
        public void TestStarted(ITestAdaptor test)
        {
        }

        /// <inheritdoc />
        public void TestFinished(ITestResultAdaptor result)
        {
        }

        /// <inheritdoc />
        public void OnError(string message)
        {
            Directory.CreateDirectory(DiagnosticsFolder.Path);
            File.WriteAllText(SummaryMarkdownPath, TestRunReportMarkdownFormatter.FormatRunError(message, DateTime.UtcNow));
        }
    }
}
