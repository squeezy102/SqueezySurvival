using System;

namespace SqueezySurvival.Core.MainMenu
{
    /// <summary>How one main menu option is shown: its label, whether it can be chosen, and why not.</summary>
    public sealed class MainMenuOptionState
    {
        /// <summary>Creates the state of a menu option.</summary>
        /// <param name="option">The option this state describes.</param>
        /// <param name="label">Text shown on the option.</param>
        /// <param name="unavailableReason">Why the option cannot be chosen, or empty when it can.</param>
        public MainMenuOptionState(MainMenuOption option, string label, string unavailableReason)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException("A menu option needs a label.", nameof(label));
            }

            Option = option;
            Label = label;
            UnavailableReason = unavailableReason ?? string.Empty;
        }

        /// <summary>The option this state describes.</summary>
        public MainMenuOption Option { get; }

        /// <summary>Text shown on the option.</summary>
        public string Label { get; }

        /// <summary>Why the option cannot be chosen; empty when it is available.</summary>
        public string UnavailableReason { get; }

        /// <summary>True when the player can choose the option.</summary>
        public bool IsAvailable => UnavailableReason.Length == 0;

        /// <summary>Text shown on the option: its label, or the reason it is unavailable.</summary>
        public string DisplayText => IsAvailable ? Label : UnavailableReason;
    }
}
