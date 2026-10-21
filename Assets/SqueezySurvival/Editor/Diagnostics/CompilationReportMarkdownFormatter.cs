using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SqueezySurvival.Editor.Diagnostics
{
    /// <summary>
    /// Writes the last compile as Markdown for people and AI assistants to read (Docs/Engineering/TestingStandards.md,
    /// Reading results without Unity): whether it succeeded, then every error and warning with its file and line.
    /// </summary>
    public static class CompilationReportMarkdownFormatter
    {
        /// <summary>The compile's messages as Markdown, errors first, then warnings, then notes.</summary>
        public static string Format(IReadOnlyList<CompilationMessageRecord> messages, DateTime finishedAtUtc)
        {
            if (messages == null)
            {
                throw new ArgumentNullException(nameof(messages));
            }

            int errors = messages.Count(message => message.Severity == CompilationMessageSeverity.Error);
            int warnings = messages.Count(message => message.Severity == CompilationMessageSeverity.Warning);
            var text = new StringBuilder();
            text.AppendLine("# Compilation");
            text.AppendLine();
            text.AppendLine($"- Result: **{(errors == 0 ? "Succeeded" : "Failed")}**");
            text.AppendLine($"- Finished: {finishedAtUtc.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)} UTC");
            text.AppendLine($"- {errors} errors, {warnings} warnings");

            AppendSection(text, "Errors", messages, CompilationMessageSeverity.Error);
            AppendSection(text, "Warnings", messages, CompilationMessageSeverity.Warning);
            AppendSection(text, "Notes", messages, CompilationMessageSeverity.Info);
            return text.ToString();
        }

        private static void AppendSection(
            StringBuilder text,
            string heading,
            IReadOnlyList<CompilationMessageRecord> messages,
            CompilationMessageSeverity severity)
        {
            List<CompilationMessageRecord> section = messages.Where(message => message.Severity == severity).ToList();
            if (section.Count == 0)
            {
                return;
            }

            text.AppendLine();
            text.AppendLine($"## {heading}");
            text.AppendLine();
            foreach (CompilationMessageRecord message in section)
            {
                text.AppendLine($"- `{Location(message)}` ({message.AssemblyName}): {message.Text}");
            }
        }

        private static string Location(CompilationMessageRecord message)
        {
            if (message.File.Length == 0)
            {
                return "(no file)";
            }

            return message.Line > 0 ? $"{message.File}:{message.Line}:{message.Column}" : message.File;
        }
    }
}
