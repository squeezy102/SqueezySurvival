namespace SqueezySurvival.Presentation.Session
{
    /// <summary>Closes the game. An interface so tests can see that a screen asked to quit without closing Unity.</summary>
    public interface IApplicationQuitter
    {
        /// <summary>Closes the game.</summary>
        void Quit();
    }
}
