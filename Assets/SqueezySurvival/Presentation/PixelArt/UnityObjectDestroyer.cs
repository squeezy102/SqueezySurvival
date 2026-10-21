using UnityEngine;

namespace SqueezySurvival.Presentation.PixelArt
{
    /// <summary>
    /// Destroys objects the game made at runtime, such as textures, in whichever mode the code runs. In Play Mode
    /// <see cref="Object.Destroy(Object)"/> frees them at the end of the frame; in Edit Mode, where Edit Mode tests run,
    /// that delayed destruction never happens and <see cref="Object.DestroyImmediate(Object)"/> is required
    /// (Unity Scripting API: Object.DestroyImmediate).
    /// </summary>
    public static class UnityObjectDestroyer
    {
        /// <summary>Destroys <paramref name="unityObject"/>. Does nothing for null.</summary>
        public static void Destroy(Object unityObject)
        {
            if (unityObject == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Object.Destroy(unityObject);
            }
            else
            {
                Object.DestroyImmediate(unityObject);
            }
        }
    }
}
