using NUnit.Framework;
using SqueezySurvival.Editor.Setup;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.TestFramework;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>
    /// A UI test panel holding the real game screen layout, so presenters are tested against the elements the game
    /// uses (Docs/Decisions/ADR-0008-Presentation-Testing.md).
    /// </summary>
    public abstract class GameScreenUITestFixture : UITestFixture
    {
        /// <summary>Loads GameScreen.uxml into the test panel before each test.</summary>
        [SetUp]
        public void LoadGameScreenLayout()
        {
            var layout = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(GameScreenSetupCommand.GameLayoutPath);
            Assert.That(layout, Is.Not.Null, $"The game screen layout is missing from {GameScreenSetupCommand.GameLayoutPath}.");

            panelSize = new Vector2(1920, 1080);
            rootVisualElement.Clear();
            layout.CloneTree(rootVisualElement);
            simulate.FrameUpdate();
        }

        /// <summary>The named element of the game screen, failing the test if the layout has no such element.</summary>
        protected T Element<T>(string name)
            where T : VisualElement
        {
            T element = rootVisualElement.Q<T>(name);
            Assert.That(element, Is.Not.Null, $"GameScreen.uxml has no {typeof(T).Name} named {name}.");
            return element;
        }
    }
}
