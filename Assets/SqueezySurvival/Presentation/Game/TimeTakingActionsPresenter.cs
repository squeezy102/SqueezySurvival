using System;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Time;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// The Explore, Wait and Rest buttons: shows their durations, issues their commands, and disables them when the run
    /// does not allow them (Docs/Design/Systems/Time.md, Passing time deliberately).
    /// </summary>
    public sealed class TimeTakingActionsPresenter : IDisposable
    {
        private readonly RunSession _session;
        private readonly Button _exploreButton;
        private readonly Button _waitButton;
        private readonly Button _restButton;

        /// <summary>Binds the action buttons in <paramref name="root"/> to <paramref name="session"/>.</summary>
        public TimeTakingActionsPresenter(VisualElement root, RunSession session)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            _session = session ?? throw new ArgumentNullException(nameof(session));
            _exploreButton = root.Q<Button>("explore-button");
            _exploreButton.text = $"Explore ({GameClockFormatter.FormatDuration(session.ExploreMinutes)})";
            _exploreButton.clicked += OnExploreClicked;
            _waitButton = BindTimePassingButton(root.Q<Button>("wait-button"), "Wait", TimePassingAction.Wait);
            _restButton = BindTimePassingButton(root.Q<Button>("rest-button"), "Rest", TimePassingAction.Rest);

            _session.PendingGroundOverflowChanged += Refresh;
            _session.Inventory.Changed += Refresh;
            _session.Hands.Changed += Refresh;
            Refresh();
        }

        /// <summary>Gives the game its default focus, the Wait button, so the screen works from the keyboard.</summary>
        public void FocusDefault() => _waitButton.Focus();

        /// <inheritdoc />
        public void Dispose()
        {
            _session.PendingGroundOverflowChanged -= Refresh;
            _session.Inventory.Changed -= Refresh;
            _session.Hands.Changed -= Refresh;
        }

        private Button BindTimePassingButton(Button button, string actionName, TimePassingAction action)
        {
            button.text = $"{actionName} ({GameClockFormatter.FormatDuration(TimePassingActions.DurationMinutes(action))})";
            button.clicked += () => _session.PassTime(action);
            return button;
        }

        private void OnExploreClicked() => _session.Explore();

        // Nothing that takes time can happen while a find waits for room on the ground, and an immobile survivor
        // can't explore (Docs/Design/Systems/Inventory.md, Load).
        private void Refresh()
        {
            bool available = !_session.HasPendingGroundOverflow;
            _exploreButton.SetEnabled(_session.CanExplore);
            _waitButton.SetEnabled(available);
            _restButton.SetEnabled(available);
        }
    }
}
