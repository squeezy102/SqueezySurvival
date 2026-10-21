using System;

namespace SqueezySurvival.Editor.Diagnostics
{
    /// <summary>How serious a compiler message is.</summary>
    public enum CompilationMessageSeverity
    {
        Error,
        Warning,
        Info,
    }

    /// <summary>One compiler error, warning or note, with where it is. Immutable.</summary>
    public sealed class CompilationMessageRecord
    {
        /// <summary>Describes a compiler message from <paramref name="assemblyName"/>.</summary>
        public CompilationMessageRecord(string assemblyName, CompilationMessageSeverity severity, string text, string file, int line, int column)
        {
            AssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
            Severity = severity;
            Text = text ?? string.Empty;
            File = file ?? string.Empty;
            Line = line;
            Column = column;
        }

        /// <summary>The assembly being compiled, such as <c>SqueezySurvival.Core</c>.</summary>
        public string AssemblyName { get; }

        /// <summary>Error, warning or note.</summary>
        public CompilationMessageSeverity Severity { get; }

        /// <summary>The compiler's message, as Unity's Console shows it.</summary>
        public string Text { get; }

        /// <summary>The source file, relative to the project folder; empty when the message has none.</summary>
        public string File { get; }

        /// <summary>The line in <see cref="File"/>, from 1; 0 when unknown.</summary>
        public int Line { get; }

        /// <summary>The column on <see cref="Line"/>, from 1; 0 when unknown.</summary>
        public int Column { get; }
    }
}
