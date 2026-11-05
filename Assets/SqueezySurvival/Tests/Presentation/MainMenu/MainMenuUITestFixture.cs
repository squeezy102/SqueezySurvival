using NUnit.Framework;
using SqueezySurvival.Editor.Setup;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.TestFramework;

namespace SqueezySurvival.Presentation.Tests.MainMenu
{
    /// <summary>
    /// A UI test panel holding the real main menu layout, so controllers are tested against the elements the game uses
    /// (Docs/Decisions/ADR-0008-Presentation-Testing.md).
    /// </summary>
    public abstract class MainMenuUITestFixture : UITestFixture
    {
        /// <summary>Loads MainMenu.uxml into the test panel before each test.</summary>
        [SetUp]
        public void LoadMainMenuLayout()
        {
            var layout = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(StartupScreensSetupCommand.MainMenuLayoutPath);
            Assert.That(layout, Is.Not.Null, $"The main menu layout is missing from {StartupScreensSetupCommand.MainMenuLayoutPath}.");

            panelSize = new Vector2(1920, 1080);
            rootVisualElement.Clear();
            layout.CloneTree(rootVisualElement);

            // The screens and notices are full-screen overlays that the menu's controller hides when it binds, so a
            // controller under test does not find the others covering its buttons.
            foreach (string overlayName in new[] { "new-game-screen", "options-screen", "start-confirmation", "not-built-notice" })
            {
                Element<VisualElement>(overlayName).style.display = DisplayStyle.None;
            }

            simulate.FrameUpdate();
        }

        /// <summary>The named element of the main menu, failing the test if the layout has no such element.</summary>
        protected T Element<T>(string name)
            where T : VisualElement
        {
            T element = rootVisualElement.Q<T>(name);
            Assert.That(element, Is.Not.Null, $"MainMenu.uxml has no {typeof(T).Name} named {name}.");
            return element;
        }
    }
}
