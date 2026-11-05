using System.Collections.Generic;
using SqueezySurvival.Presentation.Session;

namespace SqueezySurvival.Presentation.Tests.Session
{
    /// <summary>A scene loader that loads nothing and records the scenes it is asked for, in order.</summary>
    public sealed class RecordingSceneLoader : ISceneLoader
    {
        /// <summary>The names of the scenes asked for.</summary>
        public List<string> Loaded { get; } = new List<string>();

        /// <inheritdoc />
        public void Load(string sceneName) => Loaded.Add(sceneName);
    }
}
