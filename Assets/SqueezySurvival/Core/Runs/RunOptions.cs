using System;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// The run options chosen when a run starts (Docs/Design/Systems/Runs.md, Run options).
    /// Hard mode cannot be combined with Casual or Easy mode.
    /// </summary>
    public sealed class RunOptions
    {
        /// <summary>A run with no options ticked.</summary>
        public static readonly RunOptions Standard = new RunOptions(false, false, false);

        /// <summary>Creates run options.</summary>
        /// <exception cref="ArgumentException">Hard mode is combined with Casual or Easy mode.</exception>
        public RunOptions(bool isCasual, bool isEasy, bool isHard)
        {
            if (isHard && (isCasual || isEasy))
            {
                throw new ArgumentException("Hard mode cannot be combined with Casual or Easy mode.");
            }

            IsCasual = isCasual;
            IsEasy = isEasy;
            IsHard = isHard;
        }

        /// <summary>Free saving and loading; earns no points.</summary>
        public bool IsCasual { get; }

        /// <summary>An easier game for fewer points.</summary>
        public bool IsEasy { get; }

        /// <summary>A harder game for more points.</summary>
        public bool IsHard { get; }
    }
}
