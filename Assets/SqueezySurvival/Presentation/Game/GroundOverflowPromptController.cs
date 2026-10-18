using System;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Asks what to do with a find that has no room on the ground: leave it behind, or choose something on the ground
    /// to discard in its place (Docs/Design/UI/InventoryGrids.md, When the ground is full). The rules and the words
    /// come from Core; this only shows them and forwards the choice.
    /// </summary>
    public sealed class GroundOverflowPromptController : IDisposable
    {
        private const string DiscardTooltipTitle = "Discard";

        private readonly RunSession _session;
        private readonly InventoryGridView _groundView;
        private readonly VisualElement _prompt;
        private readonly Label _question;
        private readonly Button _leaveBehindButton;
        private readonly Label _leaveBehindWarning;
        private readonly Button _chooseDiscardButton;
        private readonly Label _chooseDiscardWarning;
        private readonly Label _discardHint;

        /// <summary>Binds the prompt to the run and the ground's grid. Shows itself whenever a find is waiting.</summary>
        public GroundOverflowPromptController(RunSession session, InventoryGridView groundView, VisualElement prompt, Label discardHint)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _groundView = groundView ?? throw new ArgumentNullException(nameof(groundView));
            _prompt = prompt ?? throw new ArgumentNullException(nameof(prompt));
            _discardHint = discardHint ?? throw new ArgumentNullException(nameof(discardHint));
            _question = prompt.Q<Label>("ground-overflow-question");
            _leaveBehindButton = prompt.Q<Button>("ground-overflow-leave-button");
            _leaveBehindWarning = prompt.Q<Label>("ground-overflow-leave-warning");
            _chooseDiscardButton = prompt.Q<Button>("ground-overflow-discard-button");
            _chooseDiscardWarning = prompt.Q<Label>("ground-overflow-discard-warning");

            _leaveBehindButton.clicked += () => _session.LeavePendingGroundOverflowBehind();
            _chooseDiscardButton.clicked += BeginChoosingDiscard;
            _session.PendingGroundOverflowChanged += Refresh;
            Refresh();
        }

        /// <summary>Whether the player is choosing something on the ground to discard.</summary>
        public bool IsChoosingDiscard => _groundView.IsChoosingSlot;

        /// <summary>Whether the question is showing.</summary>
        public bool IsAsking => _prompt.style.display != DisplayStyle.None;

        /// <summary>Stops choosing something to discard and asks the question again.</summary>
        public void ReturnToQuestion()
        {
            _groundView.EndSlotChoice();
            _discardHint.style.display = DisplayStyle.None;
            Refresh();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _session.PendingGroundOverflowChanged -= Refresh;
        }

        private CountedThing<ItemDefinition> Incoming => _session.PendingGroundOverflow[0];

        private void Refresh()
        {
            if (!_session.HasPendingGroundOverflow)
            {
                _groundView.EndSlotChoice();
                _discardHint.style.display = DisplayStyle.None;
                _prompt.style.display = DisplayStyle.None;
                return;
            }

            if (IsChoosingDiscard)
            {
                return;
            }

            CountedThing<ItemDefinition> incoming = Incoming;
            _question.text = GroundOverflowTextFormatter.FormatQuestion(incoming);
            _leaveBehindButton.text = GroundOverflowTextFormatter.FormatLeaveBehindChoice(incoming);
            _leaveBehindWarning.text = GroundOverflowTextFormatter.FormatLeaveBehindWarning(incoming);
            _chooseDiscardWarning.text = GroundOverflowTextFormatter.FormatDiscardChoiceWarning(incoming);
            _prompt.style.display = DisplayStyle.Flex;

            // Focus must be requested a frame after the prompt is shown for UI Toolkit to apply it.
            _prompt.schedule.Execute(() => _leaveBehindButton.Focus());
        }

        private void BeginChoosingDiscard()
        {
            CountedThing<ItemDefinition> incoming = Incoming;
            _prompt.style.display = DisplayStyle.None;
            _discardHint.text = GroundOverflowTextFormatter.FormatDiscardHint(incoming);
            _discardHint.style.display = DisplayStyle.Flex;
            _groundView.BeginSlotChoice(PreviewDiscard, Discard);
        }

        private ThingTooltipContent PreviewDiscard(int slot)
        {
            CountedEntry<ItemDefinition> discarded = _session.CurrentZone.GroundPool.EntryAtSlot(slot);
            return new ThingTooltipContent(
                DiscardTooltipTitle,
                string.Empty,
                GroundOverflowTextFormatter.FormatDiscardPreview(discarded, Incoming),
                Array.Empty<string>());
        }

        private void Discard(int slot)
        {
            _groundView.EndSlotChoice();
            _discardHint.style.display = DisplayStyle.None;
            _session.DiscardToMakeRoomForPendingGroundOverflow(slot);
        }
    }
}
