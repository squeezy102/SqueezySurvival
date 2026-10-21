using System;
using System.Globalization;
using System.Text;

namespace SqueezySurvival.Editor.Diagnostics
{
    /// <summary>
    /// Writes a test run as Markdown for people and AI assistants to read (Docs/Engineering/TestingStandards.md,
    /// Reading results without Unity): the counts, then each test that did not pass with its message, stack trace and output.
    /// </summary>
    public static class TestRunReportMarkdownFormatter
    {
        /// <summary>The report as Markdown.</summary>
        public static string Format(TestRunReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            var text = new StringBuilder();
            text.AppendLine("# Test run");
            text.AppendLine();
            text.AppendLine($"- Result: **{(report.Failed == 0 && report.Inconclusive == 0 ? "Passed" : "Failed")}**");
            text.AppendLine($"- Finished: {report.FinishedAtUtc.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)} UTC");
            text.AppendLine($"- Duration: {report.DurationSeconds.ToString("0.00", CultureInfo.InvariantCulture)} s");
            text.AppendLine($"- Total {report.Total}: {report.Passed} passed, {report.Failed} failed, {report.Skipped} skipped, {report.Inconclusive} inconclusive");

            foreach (ITestResultNode test in report.TestsNotPassed)
            {
                text.AppendLine();
                text.AppendLine($"## {test.Outcome}: {test.FullName}");
                AppendBlock(text, "Message", test.Message);
                AppendBlock(text, "Stack trace", test.StackTrace);
                AppendBlock(text, "Output", test.Output);
            }

            return text.ToString();
        }

        /// <summary>The Markdown written when a run could not start, for example because scripts do not compile.</summary>
        public static string FormatRunError(string message, DateTime atUtc)
        {
            var text = new StringBuilder();
            text.AppendLine("# Test run");
            text.AppendLine();
            text.AppendLine("- Result: **Could not run**");
            text.AppendLine($"- At: {atUtc.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)} UTC");
            AppendBlock(text, "Error", message);
            return text.ToString();
        }

        // Fenced, so stack traces and log output keep their lines and never render as Markdown.
        private static void AppendBlock(StringBuilder text, string heading, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            text.AppendLine();
            text.AppendLine($"{heading}:");
            text.AppendLine();
            text.AppendLine("```");
            text.AppendLine(content.TrimEnd());
            text.AppendLine("```");
        }
    }
}
