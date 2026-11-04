using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Content
{
    /// <summary>
    /// Content that cannot be built into a catalog. Holds every problem found, so one pass of fixes covers them all.
    /// </summary>
    public sealed class GameContentValidationException : Exception
    {
        /// <summary>Creates the exception from the problems found, each already naming its file and id.</summary>
        public GameContentValidationException(IReadOnlyList<string> problems)
            : base(BuildMessage(problems))
        {
            Problems = problems;
        }

        /// <summary>Every problem found, in the order the files were checked.</summary>
        public IReadOnlyList<string> Problems { get; }

        private static string BuildMessage(IReadOnlyList<string> problems) =>
            $"The game content has {problems.Count} problem(s):{Environment.NewLine}- " + string.Join(Environment.NewLine + "- ", problems);
    }
}
