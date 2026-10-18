using System;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// Everything chosen before a run starts, handed from the New Game screen to the game. Written once when the
    /// run's save is created and only read after that (Docs/Design/UI/StartupFlow.md, Starting the run). Immutable.
    /// </summary>
    public sealed class NewRunConfiguration
    {
        /// <summary>A run starts on Day 1 (Docs/Design/Systems/Time.md, Start of a run).</summary>
        public const int StartingDay = 1;

        /// <summary>Creates a run configuration.</summary>
        public NewRunConfiguration(DominantHand dominantHand, RunOptions runOptions)
        {
            DominantHand = dominantHand;
            RunOptions = runOptions ?? throw new ArgumentNullException(nameof(runOptions));
        }

        /// <summary>The survivor's dominant hand, which is the tool hand.</summary>
        public DominantHand DominantHand { get; }

        /// <summary>Casual, Easy and Hard mode.</summary>
        public RunOptions RunOptions { get; }

        /// <summary>Survival points this run earns.</summary>
        public PointEarningRate SurvivalPointRate => RunPointEarningRules.SurvivalPointRate(RunOptions);

        /// <summary>Challenge points this run earns.</summary>
        public PointEarningRate ChallengePointRate => RunPointEarningRules.ChallengePointRate(RunOptions);
    }
}
