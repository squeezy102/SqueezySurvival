using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SqueezySurvival.Editor.Build
{
    /// <summary>
    /// Release build settings read from Unity's command-line arguments.
    /// Usage: <c>-releaseVersion 0.0.1 -outputPath C:\path\to\Game</c>.
    /// </summary>
    public sealed class ReleaseBuildArguments
    {
        /// <summary>Flag preceding the release version.</summary>
        public const string ReleaseVersionFlag = "-releaseVersion";

        /// <summary>Flag preceding the output folder.</summary>
        public const string OutputPathFlag = "-outputPath";

        private static readonly Regex SemanticVersionPattern = new Regex(@"^\d+\.\d+\.\d+$", RegexOptions.CultureInvariant);

        private ReleaseBuildArguments(string releaseVersion, string outputPath)
        {
            ReleaseVersion = releaseVersion;
            OutputPath = outputPath;
        }

        /// <summary>Release version in <c>MAJOR.MINOR.PATCH</c> form.</summary>
        public string ReleaseVersion { get; }

        /// <summary>Folder the player is built into.</summary>
        public string OutputPath { get; }

        /// <summary>Parses release build settings from a full command line.</summary>
        /// <exception cref="ArgumentException">A required flag is missing, has no value, or the version is malformed.</exception>
        public static ReleaseBuildArguments Parse(IReadOnlyList<string> commandLineArguments)
        {
            if (commandLineArguments == null)
            {
                throw new ArgumentNullException(nameof(commandLineArguments));
            }

            string releaseVersion = GetRequiredValue(commandLineArguments, ReleaseVersionFlag);
            if (!SemanticVersionPattern.IsMatch(releaseVersion))
            {
                throw new ArgumentException(
                    $"{ReleaseVersionFlag} must be MAJOR.MINOR.PATCH, but was '{releaseVersion}'.",
                    nameof(commandLineArguments));
            }

            string outputPath = GetRequiredValue(commandLineArguments, OutputPathFlag);
            return new ReleaseBuildArguments(releaseVersion, outputPath);
        }

        private static string GetRequiredValue(IReadOnlyList<string> commandLineArguments, string flag)
        {
            for (int index = 0; index < commandLineArguments.Count - 1; index++)
            {
                if (!string.Equals(commandLineArguments[index], flag, StringComparison.Ordinal))
                {
                    continue;
                }

                string value = commandLineArguments[index + 1];
                if (!string.IsNullOrWhiteSpace(value) && !value.StartsWith("-", StringComparison.Ordinal))
                {
                    return value;
                }

                break;
            }

            throw new ArgumentException($"Missing required argument: {flag} <value>.", nameof(commandLineArguments));
        }
    }
}
