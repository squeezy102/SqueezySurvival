using SqueezySurvival.Presentation.Session;

namespace SqueezySurvival.Presentation.Tests.Session
{
    /// <summary>An application quitter that closes nothing and counts the times it is asked to.</summary>
    public sealed class RecordingApplicationQuitter : IApplicationQuitter
    {
        /// <summary>How many times quitting was asked for.</summary>
        public int QuitCount { get; private set; }

        /// <inheritdoc />
        public void Quit() => QuitCount++;
    }
}
