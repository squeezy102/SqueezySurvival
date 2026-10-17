using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace SqueezySurvival.Editor.Setup
{
    /// <summary>Shared steps of the setup commands: loading assets, building UI scenes and the build scene list.</summary>
    public static class UiSceneBuilder
    {
        public const string PanelSettingsPath = "Assets/SqueezySurvival/UI/SqueezySurvivalPanelSettings.asset";
        public const string MainMenuScenePath = "Assets/SqueezySurvival/Scenes/MainMenu.unity";
        public const string GameScenePath = "Assets/SqueezySurvival/Scenes/Game.unity";

        /// <summary>Scenes in the build, in load order. The first is the scene the game opens with.</summary>
        private static readonly string[] BuildScenePathsInOrder = { MainMenuScenePath, GameScenePath };

        /// <summary>Loads an asset, failing loudly if it is missing.</summary>
        public static T LoadRequiredAsset<T>(string path) where T : UnityEngine.Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                throw new InvalidOperationException($"Required {typeof(T).Name} not found at {path}.");
            }

            return asset;
        }

        /// <summary>
        /// Builds a scene holding a camera and one UI Toolkit document with the given components, and saves it.
        /// Replaces any existing scene at the path.
        /// </summary>
        public static void BuildUiScene(string scenePath, string layoutPath, string documentObjectName, params Type[] components)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var cameraObject = new GameObject("Main Camera") { tag = "MainCamera" };
            var mainCamera = cameraObject.AddComponent<Camera>();
            mainCamera.orthographic = true;
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = Color.black;

            var documentObject = new GameObject(documentObjectName);
            var document = documentObject.AddComponent<UIDocument>();
            document.panelSettings = LoadRequiredAsset<PanelSettings>(PanelSettingsPath);
            document.visualTreeAsset = LoadRequiredAsset<VisualTreeAsset>(layoutPath);
            foreach (Type component in components)
            {
                documentObject.AddComponent(component);
            }

            EditorSceneManager.SaveScene(scene, scenePath);
        }

        /// <summary>Sets the build scene list to every project scene that exists, in load order.</summary>
        public static void ApplyBuildSceneList()
        {
            EditorBuildSettings.scenes = BuildScenePathsInOrder
                .Where(File.Exists)
                .Select(path => new EditorBuildSettingsScene(path, true))
                .ToArray();
        }
    }
}
