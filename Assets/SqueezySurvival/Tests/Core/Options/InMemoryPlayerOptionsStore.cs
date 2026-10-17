using SqueezySurvival.Core.Options;

namespace SqueezySurvival.Core.Tests.Options
{
    /// <summary>Keeps saved options text in memory.</summary>
    public sealed class InMemoryPlayerOptionsStore : IPlayerOptionsStore
    {
        public InMemoryPlayerOptionsStore(string savedText = null)
        {
            SavedText = savedText;
        }

        public string SavedText { get; private set; }

        public int WriteCount { get; private set; }

        public bool TryRead(out string text)
        {
            text = SavedText;
            return SavedText != null;
        }

        public void Write(string text)
        {
            SavedText = text;
            WriteCount++;
        }
    }
}
