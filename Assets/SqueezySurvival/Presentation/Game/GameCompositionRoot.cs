using System;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.Randomness;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;
using SqueezySurvival.Presentation.Session;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Starts the run handed off by the New Game screen and gives it to the game screen. Without a handed-off run
    /// (for example, Play pressed with this scene open) it returns to the main menu
    /// (Docs/Design/UI/ScreenLayout.md, Wireframe stage).
    /// </summary>
    [RequireComponent(typeof(GameScreenController))]
    public sealed class GameCompositionRoot : MonoBehaviour
    {
        // Start runs after every OnEnable, so the UIDocument has built its visual tree by then.
        private void Start()
        {
            if (!GameSessionHost.Current.RunLaunchHandoff.TryTake(out NewRunConfiguration configuration))
            {
                SceneManager.LoadScene(SceneNames.MainMenu);
                return;
            }

            // A new seed each run. Saving the generator's state with the run belongs to saving (#2).
            var randomNumberSource = new SystemRandomNumberSource(Environment.TickCount);
            // Time-dependent systems in the order Architecture.md documents (Time: one path). None exist yet.
            var gameTimeAdvancer = new GameTimeAdvancer(GameClock.RunStart, Array.Empty<ITimeDependentSystem>());
            var session = new RunSession(
                configuration,
                DayPhaseSchedule.Default,
                gameTimeAdvancer,
                PlaceholderBeachZoneContent.Beach,
                new ZoneExplorer(ExplorationRules.Wireframe, randomNumberSource));
            GetComponent<GameScreenController>().Bind(session);
        }
    }
}
