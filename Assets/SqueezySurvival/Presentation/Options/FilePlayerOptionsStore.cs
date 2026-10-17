using System;
using System.IO;
using SqueezySurvival.Core.Options;

namespace SqueezySurvival.Presentation.Options
{
    /// <summary>
    /// Keeps the player's options in a text file in the player's data folder
    /// (Docs/Design/UI/GridPanels.md, Implementation notes).
    /// </summary>
    public sealed class FilePlayerOptionsStore : IPlayerOptionsStore
    {
        /// <summary>The options file's name.</summary>
        public const string FileName = "PlayerOptions.txt";

        private readonly string _filePath;

        /// <summary>Creates a store in the given folder, normally Unity's Application.persistentDataPath.</summary>
        public FilePlayerOptionsStore(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                throw new ArgumentException("Cannot be empty.", nameof(folderPath));
            }

            _filePath = Path.Combine(folderPath, FileName);
        }

        /// <inheritdoc />
        public bool TryRead(out string text)
        {
            if (!File.Exists(_filePath))
            {
                text = null;
                return false;
            }

            text = File.ReadAllText(_filePath);
            return true;
        }

        /// <inheritdoc />
        public void Write(string text)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            File.WriteAllText(_filePath, text);
        }
    }
}
