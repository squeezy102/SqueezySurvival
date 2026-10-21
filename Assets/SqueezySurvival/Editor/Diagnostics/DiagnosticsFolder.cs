namespace SqueezySurvival.Editor.Diagnostics
{
    /// <summary>
    /// Where the editor writes what it knows about the last compile and test run, so contributors and their AI
    /// assistants can read it without opening Unity (Docs/Engineering/TestingStandards.md, Reading results without
    /// Unity). Relative to the project folder; git-ignored.
    /// </summary>
    public static class DiagnosticsFolder
    {
        /// <summary>The folder, relative to the project folder.</summary>
        public const string Path = "Diagnostics";
    }
}
