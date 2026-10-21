using System;
using NUnit.Framework;
using SqueezySurvival.Editor.Diagnostics;

namespace SqueezySurvival.Editor.Tests.Diagnostics
{
    [TestFixture]
    public sealed class CompilationReportMarkdownFormatterTests
    {
        private static readonly DateTime FinishedAt = new DateTime(2026, 10, 21, 16, 30, 5, DateTimeKind.Utc);

        [Test]
        public void Format_NoMessages_SaysSucceededWithNoSections()
        {
            string markdown = CompilationReportMarkdownFormatter.Format(Array.Empty<CompilationMessageRecord>(), FinishedAt);

            Assert.That(markdown, Does.Contain("- Result: **Succeeded**"));
            Assert.That(markdown, Does.Contain("- Finished: 2026-10-21 16:30:05 UTC"));
            Assert.That(markdown, Does.Contain("- 0 errors, 0 warnings"));
            Assert.That(markdown, Does.Not.Contain("## "));
        }

        [Test]
        public void Format_WarningsOnly_StillSucceeds()
        {
            var warning = new CompilationMessageRecord("SqueezySurvival.Core", CompilationMessageSeverity.Warning, "CS0168: unused", "Assets/A.cs", 3, 9);

            string markdown = CompilationReportMarkdownFormatter.Format(new[] { warning }, FinishedAt);

            Assert.That(markdown, Does.Contain("- Result: **Succeeded**"));
            Assert.That(markdown, Does.Contain("- 0 errors, 1 warnings"));
            Assert.That(markdown, Does.Contain("## Warnings"));
        }

        [Test]
        public void Format_AnError_FailsAndListsErrorsBeforeWarningsWithFileLineAndAssembly()
        {
            var warning = new CompilationMessageRecord("SqueezySurvival.Core", CompilationMessageSeverity.Warning, "CS0168: unused", "Assets/A.cs", 3, 9);
            var error = new CompilationMessageRecord("SqueezySurvival.Presentation", CompilationMessageSeverity.Error, "CS0246: missing type", "Assets/B.cs", 12, 5);

            string markdown = CompilationReportMarkdownFormatter.Format(new[] { warning, error }, FinishedAt);

            Assert.That(markdown, Does.Contain("- Result: **Failed**"));
            Assert.That(markdown, Does.Contain("- `Assets/B.cs:12:5` (SqueezySurvival.Presentation): CS0246: missing type"));
            Assert.That(markdown.IndexOf("## Errors", StringComparison.Ordinal), Is.LessThan(markdown.IndexOf("## Warnings", StringComparison.Ordinal)));
        }

        [TestCase("", 0, "(no file)")]
        [TestCase("Assets/C.cs", 0, "Assets/C.cs")]
        public void Format_MessagesWithoutAFullLocation_SayWhatIsKnown(string file, int line, string expectedLocation)
        {
            var note = new CompilationMessageRecord("SqueezySurvival.Editor", CompilationMessageSeverity.Info, "note", file, line, 0);

            string markdown = CompilationReportMarkdownFormatter.Format(new[] { note }, FinishedAt);

            Assert.That(markdown, Does.Contain($"- `{expectedLocation}` (SqueezySurvival.Editor): note"));
            Assert.That(markdown, Does.Contain("## Notes"));
        }

        [Test]
        public void Format_Null_IsRefused()
        {
            Assert.That(() => CompilationReportMarkdownFormatter.Format(null, FinishedAt), Throws.ArgumentNullException);
        }
    }
}
