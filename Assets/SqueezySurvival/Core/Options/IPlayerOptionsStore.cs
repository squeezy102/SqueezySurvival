namespace SqueezySurvival.Core.Options
{
    /// <summary>Where the player's options are kept between sessions, as text.</summary>
    public interface IPlayerOptionsStore
    {
        /// <summary>Reads the saved options text. Returns false when nothing has been saved yet.</summary>
        bool TryRead(out string text);

        /// <summary>Saves the options text, replacing what was there.</summary>
        void Write(string text);
    }
}
