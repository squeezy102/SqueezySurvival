using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SqueezySurvival.Editor.Build
{
    /// <summary>
    /// Builds the Windows player into <c>Builds/Local</c> and runs it, for checking things that only exist in a
    /// built player, such as the splash logos. Needs no build profile asset.
    /// </summary>
    public static class LocalPlayerBuildCommand
    {
        public const string MenuPath = "SqueezySurvival/Build/Build And Run Locally";
        public const string OutputDirectory = "Builds/Local";

        [MenuItem(MenuPath)]
        public static void BuildAndRunLocally()
        {
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray(),
                locationPathName = Path.Combine(OutputDirectory, WindowsPlayerReleaseBuilder.ExecutableFileName),
                target = BuildTarget.StandaloneWindows64,
                targetGroup = BuildTargetGroup.Standalone,
                options = BuildOptions.AutoRunPlayer,
            };

            BuildSummary summary = BuildPipeline.BuildPlayer(buildPlayerOptions).summary;
            if (summary.result != BuildResult.Succeeded)
            {
                Debug.LogError($"Local build ended with {summary.result} and {summary.totalErrors} error(s).");
            }
        }
    }
}
