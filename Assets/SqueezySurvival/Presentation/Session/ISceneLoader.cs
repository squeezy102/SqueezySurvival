namespace SqueezySurvival.Presentation.Session
{
    /// <summary>Loads a scene by name. An interface so tests can see which scene a screen asks for without loading it.</summary>
    public interface ISceneLoader
    {
        /// <summary>Loads the scene named <paramref name="sceneName"/> (see <see cref="SceneNames"/>).</summary>
        void Load(string sceneName);
    }
}
