using System;

namespace SqueezySurvival.Core.Versioning
{
    /// <summary>Formats the version shown in the bottom right of the main menu (Docs/Design/UI/StartupFlow.md).</summary>
    public static class GameVersionLabelFormatter
    {
        /// <summary>Appended in the Unity Editor, where the version is not stamped by a release build.</summary>
        public const string EditorSuffix = " (editor)";

        /// <summary>Formats a version such as <c>0.0.1</c> as <c>v0.0.1</c>.</summary>
        /// <param name="version">The application version.</param>
        /// <param name="isRunningInEditor">True when running in the Unity Editor.</param>
        /// <exception cref="ArgumentException">The version is null, empty or whitespace.</exception>
        public static string Format(string version, bool isRunningInEditor)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentException("A version is required.", nameof(version));
            }

            string label = "v" + version.Trim();
            return isRunningInEditor ? label + EditorSuffix : label;
        }
    }
}
