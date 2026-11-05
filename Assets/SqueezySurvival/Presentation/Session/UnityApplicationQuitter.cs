using UnityEngine;

namespace SqueezySurvival.Presentation.Session
{
    /// <summary>Closes the game through Unity.</summary>
    public sealed class UnityApplicationQuitter : IApplicationQuitter
    {
        /// <inheritdoc />
        public void Quit()
        {
            // Application.Quit is ignored in the Unity Editor; it closes the built player.
            Application.Quit();
        }
    }
}
