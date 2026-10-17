using System;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Options;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;
using SqueezySurvival.Presentation.Options;
using SqueezySurvival.Presentation.Session;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Binds GameScreen.uxml to a <see cref="RunSession"/>. Shows state and forwards the player's actions; holds no
    /// game rules (Docs/Design/UI/ScreenLayout.md, Wireframe stage).
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public sealed class GameScreenController : MonoBehaviour
    {
        private RunSession _session;
        private Label _clockLabel;
        private Label _explorationProgressLabel;
        private GridPanelView _zoneFeaturesView;
        private GridPanelView _groundPoolView;
        private GridPanelSettingsWindowController _gridPanelSettingsWindow;
        private Button _lastChosenSettingsButton;
        private OptionsScreenController _optionsScreen;
        private Button _pauseOptionsButton;
        private Button _navOptionsButton;
        private bool _optionsOpenedFromPauseMenu;
        private InputAction _cancelAction;
        private ScrollView _eventLogEntries;
        private VisualElement _pauseMenu;
        private Button _saveAndExitButton;
        private Button _waitButton;
        private VisualElement _notBuiltNotice;
        private Label _notBuiltNoticeText;
        private Button _notBuiltNoticeCloseButton;
        private Button _lastChosenNavButton;

        /// <summary>Shows the run and connects the controls. Called once by the scene's composition root.</summary>
        /// <param name="session">The run.</param>
        /// <param name="playerOptions">The run's settings.</param>
        /// <param name="globalPreference">The player's global preference, which settings menus can capture into.</param>
        public void Bind(RunSession session, PlayerOptions playerOptions, PlayerOptions globalPreference)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            if (playerOptions == null)
            {
                throw new ArgumentNullException(nameof(playerOptions));
            }

            if (globalPreference == null)
            {
                throw new ArgumentNullException(nameof(globalPreference));
            }

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            _clockLabel = root.Q<Label>("hud-clock");
            _eventLogEntries = root.Q<ScrollView>("event-log-entries");

            // New entries are laid out a frame after they are added, so scroll once the content's size changes
            // to keep the most recent entry visible.
            _eventLogEntries.contentContainer.RegisterCallback<GeometryChangedEvent>(_ => ScrollEventLogToNewest());
            root.Q<Label>("zone-name").text = session.CurrentZone.Name;
            _explorationProgressLabel = root.Q<Label>("zone-exploration-progress");
            var gridTooltip = new GridTooltipView(root.Q<VisualElement>("grid-tooltip"));
            var dropOutcomeResolver = new GridDropOutcomeResolver(new NoThingCombinations());
            VisualElement dragLayer = root.Q<VisualElement>("game-screen-root");
            Zone zone = session.CurrentZone;
            _zoneFeaturesView = new GridPanelView(
                GridPanel.ZoneFeatures,
                playerOptions,
                new CountedCollectionGridContents<ZoneFeatureDefinition>(zone.Features, dropOutcomeResolver),
                root.Q<VisualElement>("zone-features-panel"),
                root.Q<ScrollView>("zone-features-grid"),
                root.Q<Label>("zone-features-empty"),
                gridTooltip,
                dragLayer);
            _groundPoolView = new GridPanelView(
                GridPanel.GroundPool,
                playerOptions,
                new CountedCollectionGridContents<ItemDefinition>(zone.GroundPool, dropOutcomeResolver),
                root.Q<VisualElement>("ground-pool-panel"),
                root.Q<ScrollView>("ground-pool-grid"),
                root.Q<Label>("ground-pool-empty"),
                gridTooltip,
                dragLayer);

            _gridPanelSettingsWindow = new GridPanelSettingsWindowController(root.Q<VisualElement>("grid-panel-settings-window"), playerOptions, globalPreference);
            _gridPanelSettingsWindow.Closed += HideGridPanelSettings;
            BindPanelSettingsButton(root.Q<Button>("zone-features-settings-button"), GridPanel.ZoneFeatures, "Zone features");
            BindPanelSettingsButton(root.Q<Button>("ground-pool-settings-button"), GridPanel.GroundPool, "On the ground");

            _optionsScreen = new OptionsScreenController(root.Q<VisualElement>("options-screen"), playerOptions, globalPreference);
            _optionsScreen.Closed += HideOptionsScreen;
            _pauseOptionsButton = root.Q<Button>("pause-options-button");
            _pauseOptionsButton.clicked += () => ShowOptionsScreen(openedFromPauseMenu: true);

            Button exploreButton = root.Q<Button>("explore-button");
            exploreButton.text = $"Explore ({GameClockFormatter.FormatDuration(session.ExploreMinutes)})";
            exploreButton.clicked += () => _session.Explore();

            _waitButton = BindTimePassingButton(root.Q<Button>("wait-button"), "Wait", TimePassingAction.Wait);
            BindTimePassingButton(root.Q<Button>("rest-button"), "Rest", TimePassingAction.Rest);

            _notBuiltNotice = root.Q<VisualElement>("not-built-notice");
            _notBuiltNoticeText = root.Q<Label>("not-built-notice-text");
            _notBuiltNoticeCloseButton = root.Q<Button>("not-built-notice-close-button");
            _notBuiltNoticeCloseButton.clicked += HideNotBuiltNotice;
            _notBuiltNotice.style.display = DisplayStyle.None;
            _navOptionsButton = root.Q<Button>("nav-options-button");
            _navOptionsButton.clicked += () => ShowOptionsScreen(openedFromPauseMenu: false);
            root.Q<VisualElement>("nav-bar").Query<Button>().Where(button => button != _navOptionsButton).ForEach(BindNavButton);

            _pauseMenu = root.Q<VisualElement>("pause-menu");
            _saveAndExitButton = root.Q<Button>("save-and-exit-button");

            // Saving is not built yet, so Save and exit only exits (Docs/Design/UI/ScreenLayout.md, Wireframe stage).
            _saveAndExitButton.clicked += () => SceneManager.LoadScene(SceneNames.MainMenu);
            _pauseMenu.style.display = DisplayStyle.None;
            // Escape is read from the keyboard directly rather than from UI navigation, which goes only to the
            // focused element and is lost once a non-focusable grid cell is clicked (#48).
            _cancelAction = new InputAction("Cancel", InputActionType.Button, "<Keyboard>/escape");
            _cancelAction.performed += _ => OnCancel();
            _cancelAction.Enable();

            foreach (EventLogEntry entry in session.EventLog.Entries)
            {
                AppendLogEntry(entry);
            }

            session.EventLog.EntryAdded += AppendLogEntry;
            session.ClockAdvanced += _ => RefreshClock();
            RefreshClock();

            zone.ExplorationProgressChanged += RefreshExplorationProgress;
            RefreshExplorationProgress(zone.ExplorationProgress);

            // Focus must be requested a frame after layout for UI Toolkit to apply it.
            root.schedule.Execute(() => _waitButton.Focus());
        }

        private void OnDestroy()
        {
            _cancelAction?.Disable();
            _cancelAction?.Dispose();
            if (_session != null)
            {
                _session.EventLog.EntryAdded -= AppendLogEntry;
                _session.CurrentZone.ExplorationProgressChanged -= RefreshExplorationProgress;
            }

            _zoneFeaturesView?.Dispose();
            _groundPoolView?.Dispose();
            _gridPanelSettingsWindow?.Dispose();
            _optionsScreen?.Dispose();
        }

        private Button BindTimePassingButton(Button button, string actionName, TimePassingAction action)
        {
            button.text = $"{actionName} ({GameClockFormatter.FormatDuration(TimePassingActions.DurationMinutes(action))})";
            button.clicked += () => _session.PassTime(action);
            return button;
        }

        private void RefreshClock()
        {
            _clockLabel.text = GameClockFormatter.FormatHudClock(_session.Clock, _session.DayPhaseSchedule);
        }

        private void RefreshExplorationProgress(ExplorationProgress progress)
        {
            _explorationProgressLabel.text = $"Explored {progress.WholePercent}%";
        }

        private void BindPanelSettingsButton(Button button, GridPanel panel, string panelName)
        {
            button.clicked += () =>
            {
                _lastChosenSettingsButton = button;
                _gridPanelSettingsWindow.Show(panel, panelName);
            };
        }

        private void HideGridPanelSettings()
        {
            _gridPanelSettingsWindow.Hide();
            _lastChosenSettingsButton?.Focus();
        }

        private void ShowOptionsScreen(bool openedFromPauseMenu)
        {
            _optionsOpenedFromPauseMenu = openedFromPauseMenu;
            _pauseMenu.style.display = DisplayStyle.None;
            _optionsScreen.Show();
        }

        // Closing Options returns to where it was opened: the pause menu, or the game.
        private void HideOptionsScreen()
        {
            _optionsScreen.Hide();
            if (_optionsOpenedFromPauseMenu)
            {
                _pauseMenu.style.display = DisplayStyle.Flex;
                _pauseOptionsButton.Focus();
            }
            else
            {
                _navOptionsButton.Focus();
            }
        }

        private void AppendLogEntry(EventLogEntry entry)
        {
            string time = GameClockFormatter.FormatTimeOfDay(entry.Time.MinuteOfDay);
            var line = new Label($"Day {entry.Time.Day}, {time}: {entry.Text}");
            line.AddToClassList("event-log-entry");
            _eventLogEntries.Add(line);
        }

        private void ScrollEventLogToNewest()
        {
            _eventLogEntries.verticalScroller.value = _eventLogEntries.verticalScroller.highValue;
        }

        // Every window behind the nav bar is unbuilt at the wireframe stage (Docs/Design/UI/ScreenLayout.md).
        private void BindNavButton(Button button)
        {
            button.clicked += () =>
            {
                _lastChosenNavButton = button;
                _notBuiltNoticeText.text = $"{button.text} isn't built yet.";
                _notBuiltNotice.style.display = DisplayStyle.Flex;
                _notBuiltNoticeCloseButton.Focus();
            };
        }

        private void HideNotBuiltNotice()
        {
            _notBuiltNotice.style.display = DisplayStyle.None;
            _lastChosenNavButton?.Focus();
        }

        private void OnCancel()
        {
            if (_zoneFeaturesView.IsDragging || _groundPoolView.IsDragging)
            {
                // Escape during a drag only cancels the drag (Docs/Design/UI/DragAndDrop.md, Dragging).
                _zoneFeaturesView.CancelDrag();
                _groundPoolView.CancelDrag();
            }
            else if (_notBuiltNotice.style.display != DisplayStyle.None)
            {
                HideNotBuiltNotice();
            }
            else if (_gridPanelSettingsWindow.IsVisible)
            {
                HideGridPanelSettings();
            }
            else if (_optionsScreen.IsVisible)
            {
                HideOptionsScreen();
            }
            else
            {
                TogglePauseMenu();
            }
        }

        private void TogglePauseMenu()
        {
            if (_pauseMenu.style.display == DisplayStyle.None)
            {
                _pauseMenu.style.display = DisplayStyle.Flex;
                _pauseOptionsButton.Focus();
            }
            else
            {
                HidePauseMenu();
            }
        }

        private void HidePauseMenu()
        {
            _pauseMenu.style.display = DisplayStyle.None;
            _waitButton.Focus();
        }
    }
}
