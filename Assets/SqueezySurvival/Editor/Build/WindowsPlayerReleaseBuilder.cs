using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace SqueezySurvival.Editor.Build
{
    /// <summary>
    /// Builds the Windows player for a release. Invoked by Tools/Publish-Release.ps1 through
    /// <c>-executeMethod SqueezySurvival.Editor.Build.WindowsPlayerReleaseBuilder.BuildFromCommandLine</c>.
    /// </summary>
    public static class WindowsPlayerReleaseBuilder
    {
        /// <summary>File name of the built player executable. The launcher expects this name.</summary>
        public const string ExecutableFileName = "SqueezySurvival.exe";

        /// <summary>
        /// Command-line entry point. Throws on failure, which makes Unity exit with code 1 in batch mode.
        /// </summary>
        public static void BuildFromCommandLine()
        {
            ReleaseBuildArguments arguments = ReleaseBuildArguments.Parse(Environment.GetCommandLineArgs());
            BuildSummary summary = Build(arguments).summary;
            if (summary.result != BuildResult.Succeeded)
            {
                throw new BuildFailedException(
                    $"Windows player build {arguments.ReleaseVersion} ended with {summary.result} and {summary.totalErrors} error(s).");
            }
        }

        /// <summary>
        /// Builds every enabled scene in Build Settings as a 64-bit Windows player stamped with the release version.
        /// The project's own version setting is restored afterwards so the build leaves no changes behind.
        /// </summary>
        public static BuildReport Build(ReleaseBuildArguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            string originalBundleVersion = PlayerSettings.bundleVersion;
            PlayerSettings.bundleVersion = arguments.ReleaseVersion;
            try
            {
                var buildPlayerOptions = new BuildPlayerOptions
                {
                    scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray(),
                    locationPathName = Path.Combine(arguments.OutputPath, ExecutableFileName),
                    target = BuildTarget.StandaloneWindows64,
                    targetGroup = BuildTargetGroup.Standalone,
                    options = BuildOptions.StrictMode,
                };

                return BuildPipeline.BuildPlayer(buildPlayerOptions);
            }
            finally
            {
                PlayerSettings.bundleVersion = originalBundleVersion;
            }
        }
    }
}
