using UnityEngine.SceneManagement;

namespace SqueezySurvival.Presentation.Session
{
    /// <summary>Loads scenes through Unity's scene manager.</summary>
    public sealed class UnitySceneLoader : ISceneLoader
    {
        /// <inheritdoc />
        public void Load(string sceneName) => SceneManager.LoadScene(sceneName);
    }
}
