using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Compilation;

namespace SqueezySurvival.Editor.Diagnostics
{
    /// <summary>
    /// After every script compile, writes its errors and warnings to the <see cref="DiagnosticsFolder"/>, so a compile
    /// that fails, which stops tests from running at all, can be read without opening Unity
    /// (Docs/Engineering/TestingStandards.md, Reading results without Unity). Unity raises
    /// <see cref="CompilationPipeline.assemblyCompilationFinished"/> for each assembly and then
    /// <see cref="CompilationPipeline.compilationFinished"/> once (Unity Scripting API: CompilationPipeline).
    /// </summary>
    [InitializeOnLoad]
    public static class CompilationResultsExporter
    {
        /// <summary>The last compile's errors and warnings, in Markdown.</summary>
        public const string ReportMarkdownPath = DiagnosticsFolder.Path + "/latest-compilation.md";

        private static readonly List<CompilationMessageRecord> MessagesThisCompile = new List<CompilationMessageRecord>();

        static CompilationResultsExporter()
        {
            CompilationPipeline.compilationStarted += OnCompilationStarted;
            CompilationPipeline.assemblyCompilationFinished += OnAssemblyCompilationFinished;
            CompilationPipeline.compilationFinished += OnCompilationFinished;
        }

        private static void OnCompilationStarted(object context)
        {
            MessagesThisCompile.Clear();
        }

        private static void OnAssemblyCompilationFinished(string assemblyPath, CompilerMessage[] messages)
        {
            string assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
            foreach (CompilerMessage message in messages)
            {
                MessagesThisCompile.Add(new CompilationMessageRecord(
                    assemblyName, ToSeverity(message.type), message.message, message.file, message.line, message.column));
            }
        }

        private static void OnCompilationFinished(object context)
        {
            Directory.CreateDirectory(DiagnosticsFolder.Path);
            File.WriteAllText(ReportMarkdownPath, CompilationReportMarkdownFormatter.Format(MessagesThisCompile, DateTime.UtcNow));
        }

        private static CompilationMessageSeverity ToSeverity(CompilerMessageType type)
        {
            switch (type)
            {
                case CompilerMessageType.Error:
                    return CompilationMessageSeverity.Error;
                case CompilerMessageType.Warning:
                    return CompilationMessageSeverity.Warning;
                default:
                    return CompilationMessageSeverity.Info;
            }
        }
    }
}
