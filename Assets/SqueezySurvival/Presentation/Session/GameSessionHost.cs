using SqueezySurvival.Core.Runs;
using UnityEngine;

namespace SqueezySurvival.Presentation.Session
{
    /// <summary>
    /// Holds the services that outlive a single scene, such as the <see cref="RunLaunchHandoff"/> that carries a
    /// new run's configuration from the main menu into the game. Created once before the first scene loads and
    /// kept across scene changes. Scene composition roots read their shared services from <see cref="Current"/>.
    /// </summary>
    public sealed class GameSessionHost : MonoBehaviour
    {
        /// <summary>The session host for this play session.</summary>
        public static GameSessionHost Current { get; private set; }

        /// <summary>Carries a new run's configuration into the game scene.</summary>
        public RunLaunchHandoff RunLaunchHandoff { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateBeforeFirstScene()
        {
            var hostObject = new GameObject(nameof(GameSessionHost));
            DontDestroyOnLoad(hostObject);
            Current = hostObject.AddComponent<GameSessionHost>();
            Current.RunLaunchHandoff = new RunLaunchHandoff();
        }
    }
}
