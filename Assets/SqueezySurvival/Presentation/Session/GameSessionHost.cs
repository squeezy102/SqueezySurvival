using SqueezySurvival.Core.Options;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Presentation.Options;
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

        /// <summary>The player's options, shared by every screen and saved after each change (Docs/Design/UI/Options.md).</summary>
        public PlayerOptions PlayerOptions => _playerOptionsPersistence.Options;

        private PlayerOptionsPersistence _playerOptionsPersistence;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateBeforeFirstScene()
        {
            var hostObject = new GameObject(nameof(GameSessionHost));
            DontDestroyOnLoad(hostObject);
            Current = hostObject.AddComponent<GameSessionHost>();
            Current.RunLaunchHandoff = new RunLaunchHandoff();
            Current._playerOptionsPersistence = PlayerOptionsPersistence.Load(new FilePlayerOptionsStore(Application.persistentDataPath));
        }
    }
}
