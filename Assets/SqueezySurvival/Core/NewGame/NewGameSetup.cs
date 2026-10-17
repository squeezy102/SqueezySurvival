using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.NewGame
{
    /// <summary>
    /// The choices on the New Game screen, with the run option rules applied as they change
    /// (Docs/Design/UI/StartupFlow.md, New Game screen).
    /// </summary>
    public sealed class NewGameSetup
    {
        /// <summary>Shown on Start until a dominant hand is chosen.</summary>
        public const string ChooseDominantHandReason = "Choose a dominant hand";

        /// <summary>The survivor's dominant hand. Nothing until the player chooses; there is no default.</summary>
        public DominantHand? DominantHand { get; set; }

        /// <summary>True when every required choice has been made, so the run can start.</summary>
        public bool CanStart => DominantHand.HasValue;

        /// <summary>Why the run cannot start yet, or empty when it can.</summary>
        public string StartUnavailableReason => CanStart ? string.Empty : ChooseDominantHandReason;

        /// <summary>True when Casual mode is ticked.</summary>
        public bool IsCasual { get; private set; }

        /// <summary>True when Easy mode is ticked.</summary>
        public bool IsEasy { get; private set; }

        /// <summary>True when Hard mode is ticked.</summary>
        public bool IsHard { get; private set; }

        /// <summary>
        /// True when Hard mode is ticked, so Casual mode is shown dimmed. Ticking it anyway clears Hard mode.
        /// </summary>
        public bool IsCasualInConflict => IsHard;

        /// <summary>
        /// True when Hard mode is ticked, so Easy mode is shown dimmed. Ticking it anyway clears Hard mode.
        /// </summary>
        public bool IsEasyInConflict => IsHard;

        /// <summary>
        /// True when Casual or Easy mode is ticked, so Hard mode is shown dimmed. Ticking it anyway clears both.
        /// </summary>
        public bool IsHardInConflict => IsCasual || IsEasy;

        /// <summary>Ticks or clears Casual mode. Ticking it clears Hard mode.</summary>
        public void SetCasual(bool isTicked)
        {
            IsCasual = isTicked;
            if (isTicked)
            {
                IsHard = false;
            }
        }

        /// <summary>Ticks or clears Easy mode. Ticking it clears Hard mode.</summary>
        public void SetEasy(bool isTicked)
        {
            IsEasy = isTicked;
            if (isTicked)
            {
                IsHard = false;
            }
        }

        /// <summary>Ticks or clears Hard mode. Ticking it clears Casual and Easy mode.</summary>
        public void SetHard(bool isTicked)
        {
            IsHard = isTicked;
            if (isTicked)
            {
                IsCasual = false;
                IsEasy = false;
            }
        }

        /// <summary>The run options currently chosen.</summary>
        public RunOptions ToRunOptions() => new RunOptions(IsCasual, IsEasy, IsHard);

        /// <summary>Turns the choices into the configuration the run starts with.</summary>
        /// <exception cref="System.InvalidOperationException">A required choice has not been made.</exception>
        public NewRunConfiguration CreateRunConfiguration()
        {
            if (!DominantHand.HasValue)
            {
                throw new System.InvalidOperationException(ChooseDominantHandReason);
            }

            return new NewRunConfiguration(DominantHand.Value, ToRunOptions());
        }
    }
}
