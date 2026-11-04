using System.Collections.Generic;

namespace SqueezySurvival.Core.Content
{
    /// <summary>Collects what is wrong with the content, so every problem is reported at once.</summary>
    internal sealed class GameContentProblems
    {
        private readonly List<string> _problems = new List<string>();

        public int Count => _problems.Count;

        /// <summary>Adds a problem: "Items.json, item 'stone': description is empty".</summary>
        public void Add(string fileName, string subject, string problem) => _problems.Add($"{fileName}, {subject}: {problem}");

        public GameContentValidationException ToException() => new GameContentValidationException(_problems.ToArray());
    }
}
