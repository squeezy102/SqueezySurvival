using System;

namespace SqueezySurvival.Presentation.Content
{
    /// <summary>A content file that cannot be read as JSON in the shape the game expects. The message names the file.</summary>
    public sealed class GameContentFormatException : Exception
    {
        /// <summary>Creates the exception for a file, with the reason it could not be read.</summary>
        public GameContentFormatException(string fileName, string reason, Exception innerException = null)
            : base($"{fileName}: {reason}", innerException)
        {
            FileName = fileName;
        }

        /// <summary>The file's name, such as "Items.json" or "Zones/Beach.json".</summary>
        public string FileName { get; }
    }
}
