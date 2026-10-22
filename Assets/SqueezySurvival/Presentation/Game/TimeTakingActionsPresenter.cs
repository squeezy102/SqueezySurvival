using System;
using SqueezySurvival.Core.Items;
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
        private readonly ZoneExplorationService _zoneExploration;
        private readonly TimePassingService _timePassing;
        private readonly SurvivorInventoryService _survivorInventory;
        private readonly GroundInventoryService _groundInventory;
        private readonly Button _exploreButton;
        private readonly Button _waitButton;
        private readonly Button _restButton;

        /// <summary>
        /// Binds the action buttons in <paramref name="root"/>. They are refreshed when what the survivor carries or the
        /// finds waiting for room change, the two things that decide whether they are available.
        /// </summary>
        public TimeTakingActionsPresenter(
            VisualElement root,
            ZoneExplorationService zoneExploration,
            TimePassingService timePassing,
            SurvivorInventoryService survivorInventory,
            GroundInventoryService groundInventory)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            _zoneExploration = zoneExploration ?? throw new ArgumentNullException(nameof(zoneExploration));
            _timePassing = timePassing ?? throw new ArgumentNullException(nameof(timePassing));
            _survivorInventory = survivorInventory ?? throw new ArgumentNullException(nameof(survivorInventory));
            _groundInventory = groundInventory ?? throw new ArgumentNullException(nameof(groundInventory));

            _exploreButton = root.Q<Button>("explore-button");
            _exploreButton.text = $"Explore ({GameClockFormatter.FormatDuration(zoneExploration.ExploreMinutes)})";
            _exploreButton.clicked += OnExploreClicked;
            _waitButton = BindTimePassingButton(root.Q<Button>("wait-button"), "Wait", TimePassingAction.Wait);
            _restButton = BindTimePassingButton(root.Q<Button>("rest-button"), "Rest", TimePassingAction.Rest);

            _groundInventory.PendingOverflowChanged += Refresh;
            _survivorInventory.Inventory.Changed += OnCarriedThingsChanged;
            _survivorInventory.Hands.Changed += OnCarriedThingsChanged;
            Refresh();
        }

        /// <summary>Gives the game its default focus, the Wait button, so the screen works from the keyboard.</summary>
        public void FocusDefault() => _waitButton.Focus();

        /// <inheritdoc />
        public void Dispose()
        {
            _groundInventory.PendingOverflowChanged -= Refresh;
            _survivorInventory.Inventory.Changed -= OnCarriedThingsChanged;
            _survivorInventory.Hands.Changed -= OnCarriedThingsChanged;
        }

        private Button BindTimePassingButton(Button button, string actionName, TimePassingAction action)
        {
            button.text = $"{actionName} ({GameClockFormatter.FormatDuration(TimePassingActions.DurationMinutes(action))})";
            button.clicked += () => _timePassing.PassTime(action);
            return button;
        }

        private void OnExploreClicked() => _zoneExploration.Explore();

        // Whether actions are allowed depends on the total load, not on which slots changed.
        private void OnCarriedThingsChanged(object sender, CountedCollectionChange change) => Refresh();

        private void Refresh()
        {
            bool canPassTime = _timePassing.CanPassTime;
            _exploreButton.SetEnabled(_zoneExploration.CanExplore);
            _waitButton.SetEnabled(canPassTime);
            _restButton.SetEnabled(canPassTime);
        }
    }
}
