using System;
using System.Linq;
using SqueezySurvival.Presentation.Game;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SqueezySurvival.Editor.Setup
{
    /// <summary>
    /// Creates the game scene, gives its composition root the content files, and adds the scene to the build
    /// (Docs/Design/UI/ScreenLayout.md, Wireframe stage; Docs/Decisions/ADR-0009-Data-Driven-Content.md).
    /// Run Configure Startup Screens first; it creates the shared panel settings. Safe to run again.
    /// </summary>
    public static class GameScreenSetupCommand
    {
        public const string MenuPath = "SqueezySurvival/Setup/Configure Game Screen";
        public const string GameLayoutPath = "Assets/SqueezySurvival/UI/Game/GameScreen.uxml";
        public const string ContentFolderPath = "Assets/SqueezySurvival/Content";
        public const string ItemsFilePath = ContentFolderPath + "/Items.json";
        public const string ZoneFeaturesFilePath = ContentFolderPath + "/ZoneFeatures.json";
        public const string ZoneFilesFolderPath = ContentFolderPath + "/Zones";

        // The serialized fields of GameCompositionRoot that hold the content files.
        private const string ItemsFileFieldName = "_itemsFile";
        private const string ZoneFeaturesFileFieldName = "_zoneFeaturesFile";
        private const string ZoneFilesFieldName = "_zoneFiles";

        [MenuItem(MenuPath)]
        public static void ConfigureGameScreen()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            UiSceneBuilder.BuildUiScene(
                UiSceneBuilder.GameScenePath,
                GameLayoutPath,
                "Game Screen",
                AssignContentFiles,
                typeof(GameCompositionRoot));
            UiSceneBuilder.ApplyBuildSceneList();

            Debug.Log($"Game screen configured. Game scene: {UiSceneBuilder.GameScenePath}.");
        }

        /// <summary>Sets the composition root's content references: Items.json, ZoneFeatures.json and every file under Zones/.</summary>
        private static void AssignContentFiles(GameObject documentObject)
        {
            var compositionRoot = new SerializedObject(documentObject.GetComponent<GameCompositionRoot>());
            FindRequiredField(compositionRoot, ItemsFileFieldName).objectReferenceValue =
                UiSceneBuilder.LoadRequiredAsset<TextAsset>(ItemsFilePath);
            FindRequiredField(compositionRoot, ZoneFeaturesFileFieldName).objectReferenceValue =
                UiSceneBuilder.LoadRequiredAsset<TextAsset>(ZoneFeaturesFilePath);

            string[] zoneFilePaths = FindZoneFilePaths();
            SerializedProperty zoneFiles = FindRequiredField(compositionRoot, ZoneFilesFieldName);
            zoneFiles.arraySize = zoneFilePaths.Length;
            for (int index = 0; index < zoneFilePaths.Length; index++)
            {
                zoneFiles.GetArrayElementAtIndex(index).objectReferenceValue = UiSceneBuilder.LoadRequiredAsset<TextAsset>(zoneFilePaths[index]);
            }

            compositionRoot.ApplyModifiedPropertiesWithoutUndo();
        }

        /// <summary>The path of every zone file under Zones/, in name order.</summary>
        public static string[] FindZoneFilePaths() => AssetDatabase
            .FindAssets("t:TextAsset", new[] { ZoneFilesFolderPath })
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(path => path.EndsWith(".json", StringComparison.Ordinal))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();

        private static SerializedProperty FindRequiredField(SerializedObject serializedObject, string fieldName) =>
            serializedObject.FindProperty(fieldName)
            ?? throw new InvalidOperationException($"{nameof(GameCompositionRoot)} has no serialized field named {fieldName}.");
    }
}
