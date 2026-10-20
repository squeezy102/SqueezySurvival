using System;
using System.Collections.Generic;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.InventoryGrids;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Navigation;
using SqueezySurvival.Core.PixelArt;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;
using SqueezySurvival.Presentation.Icons;
using SqueezySurvival.Presentation.Options;
using SqueezySurvival.Presentation.PixelArt;
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
        private Label _clockDayLabel;
        private Label _clockTimeLabel;
        private Label _clockPhaseLabel;
        private Label _explorationProgressLabel;
        private InventoryGridView _zoneFeaturesView;
        private InventoryGridView _groundPoolView;
        private InventoryGridView _inventoryView;
        private InventoryGridView _handsView;
        private GroundOverflowPromptController _groundOverflowPrompt;
        private PixelArtTextureCache _pixelArtTextures;
        private DayNightDialView _dayNightDial;
        private Button _exploreButton;
        private Button _restButton;
        private OptionsScreenController _optionsScreen;
        private Button _pauseOptionsButton;
        private Button _navOptionsButton;
        private bool _optionsOpenedFromPauseMenu;
        private InputAction _cancelAction;
        private EventLogView _eventLogView;
        private VisualElement _pauseMenu;
        private Button _saveAndExitButton;
        private Button _waitButton;
        private VisualElement _notBuiltNotice;
        private Label _notBuiltNoticeText;
        private Button _notBuiltNoticeCloseButton;
        private Button _lastChosenNavButton;

        /// <summary>Shows the run and connects the controls. Called once by the scene's composition root.</summary>
        /// <param name="session">The run.</param>
        public void Bind(RunSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            _clockDayLabel = root.Q<Label>("hud-clock-day");
            _clockTimeLabel = root.Q<Label>("hud-clock-time");
            _clockPhaseLabel = root.Q<Label>("hud-clock-phase");
            root.Q<Label>("zone-name").text = session.CurrentZone.Name;
            _explorationProgressLabel = root.Q<Label>("zone-exploration-progress");
            _pixelArtTextures = new PixelArtTextureCache();
            var gridTooltip = new GridTooltipView(root.Q<VisualElement>("grid-tooltip"), _pixelArtTextures);
            var skin = new PixelArtInterfaceSkin(_pixelArtTextures);
            var dropOutcomeResolver = new GridDropOutcomeResolver(new NoThingCombinations());
            VisualElement dragLayer = root.Q<VisualElement>("game-screen-root");
            Zone zone = session.CurrentZone;
            _zoneFeaturesView = new InventoryGridView(
                InventoryGridShapes.ZoneFeatures,
                FixedGridHorizontalAlignment.Center,
                new CountedCollectionInventoryGridContents<ZoneFeatureDefinition>(zone.Features, dropOutcomeResolver),
                root.Q<VisualElement>("zone-features-grid"),
                gridTooltip,
                dragLayer,
                skin);
            var groundContents = new CountedCollectionInventoryGridContents<ItemDefinition>(zone.GroundPool, dropOutcomeResolver);
            _groundPoolView = new InventoryGridView(
                InventoryGridShapes.Ground,
                FixedGridHorizontalAlignment.Center,
                groundContents,
                root.Q<VisualElement>("ground-pool-grid"),
                gridTooltip,
                dragLayer,
                skin);
            new InventoryGridSortController(groundContents, root.Q<Button>("ground-pool-sort-button"), root.Q<Button>("ground-pool-sort-menu-button"));

            var inventoryContents = new CountedCollectionInventoryGridContents<ItemDefinition>(session.Inventory, dropOutcomeResolver);
            _inventoryView = new InventoryGridView(
                InventoryGridShapes.Inventory,
                FixedGridHorizontalAlignment.Left,
                inventoryContents,
                root.Q<VisualElement>("inventory-grid"),
                gridTooltip,
                dragLayer,
                skin);
            new InventoryGridSortController(inventoryContents, root.Q<Button>("inventory-sort-button"), root.Q<Button>("inventory-sort-menu-button"));
            VisualElement handsGrid = root.Q<VisualElement>("hands-grid");
            _handsView = new InventoryGridView(
                InventoryGridShapes.Hands,
                FixedGridHorizontalAlignment.Left,
                new CountedCollectionInventoryGridContents<ItemDefinition>(session.Hands, dropOutcomeResolver),
                handsGrid,
                gridTooltip,
                dragLayer,
                skin,
                slot => _pixelArtTextures.HandGhost(HandSlots.HandAt(slot)));

            // The hands are as wide as their cells need at the row's height, so their cells match the inventory's.
            // Bind runs after the first layout pass, whose geometry event has already fired, so the width is also
            // set from the current layout (#51).
            VisualElement inventoryRow = handsGrid.parent;
            inventoryRow.RegisterCallback<GeometryChangedEvent>(changed => SizeHandsGrid(handsGrid, changed.newRect.height));
            SizeHandsGrid(handsGrid, inventoryRow.layout.height);

            _groundOverflowPrompt = new GroundOverflowPromptController(
                session, _groundPoolView, root.Q<VisualElement>("ground-overflow-prompt"), root.Q<Label>("ground-discard-hint"));
            session.PendingGroundOverflowChanged += RefreshTimeTakingActions;

            _optionsScreen = new OptionsScreenController(root.Q<VisualElement>("options-screen"));
            _optionsScreen.Closed += HideOptionsScreen;
            _pauseOptionsButton = root.Q<Button>("pause-options-button");
            _pauseOptionsButton.clicked += () => ShowOptionsScreen(openedFromPauseMenu: true);

            _exploreButton = root.Q<Button>("explore-button");
            _exploreButton.text = $"Explore ({GameClockFormatter.FormatDuration(session.ExploreMinutes)})";
            _exploreButton.clicked += () => _session.Explore();

            _waitButton = BindTimePassingButton(root.Q<Button>("wait-button"), "Wait", TimePassingAction.Wait);
            _restButton = BindTimePassingButton(root.Q<Button>("rest-button"), "Rest", TimePassingAction.Rest);

            _notBuiltNotice = root.Q<VisualElement>("not-built-notice");
            _notBuiltNoticeText = root.Q<Label>("not-built-notice-text");
            _notBuiltNoticeCloseButton = root.Q<Button>("not-built-notice-close-button");
            _notBuiltNoticeCloseButton.clicked += HideNotBuiltNotice;
            _notBuiltNotice.style.display = DisplayStyle.None;
            _navOptionsButton = root.Q<Button>("nav-options-button");
            BindNavIconAndTooltip(_navOptionsButton, NavigationBarEntries.Options, gridTooltip);
            _navOptionsButton.clicked += () => ShowOptionsScreen(openedFromPauseMenu: false);
            foreach (NavigationBarEntry entry in NavigationBarEntries.GameplayWindows)
            {
                Button button = root.Q<Button>($"nav-{entry.Key}-button");
                BindNavIconAndTooltip(button, entry, gridTooltip);
                BindNotBuiltNavButton(button, entry);
            }

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

            _eventLogView = new EventLogView(root.Q<VisualElement>("event-log-panel"), session.EventLog, _pixelArtTextures, new UnreadWarningTracker());
            _dayNightDial = new DayNightDialView(root.Q<VisualElement>("hud-day-night-dial"), session.Clock);
            session.ClockAdvanced += _ => RefreshClock();
            RefreshClock();

            zone.ExplorationProgressChanged += RefreshExplorationProgress;
            RefreshExplorationProgress(zone.ExplorationProgress);

            skin.ApplyTo(root);
            skin.ApplyBackdrop(dragLayer);

            // Focus must be requested a frame after layout for UI Toolkit to apply it.
            root.schedule.Execute(() => _waitButton.Focus());
        }

        private void OnDestroy()
        {
            _cancelAction?.Disable();
            _cancelAction?.Dispose();
            if (_session != null)
            {
                _session.CurrentZone.ExplorationProgressChanged -= RefreshExplorationProgress;
                _session.PendingGroundOverflowChanged -= RefreshTimeTakingActions;
            }

            _eventLogView?.Dispose();
            _zoneFeaturesView?.Dispose();
            _groundPoolView?.Dispose();
            _inventoryView?.Dispose();
            _handsView?.Dispose();
            _groundOverflowPrompt?.Dispose();
            _pixelArtTextures?.Dispose();
            _dayNightDial?.Dispose();
        }

        private Button BindTimePassingButton(Button button, string actionName, TimePassingAction action)
        {
            button.text = $"{actionName} ({GameClockFormatter.FormatDuration(TimePassingActions.DurationMinutes(action))})";
            button.clicked += () => _session.PassTime(action);
            return button;
        }

        private void RefreshClock()
        {
            _dayNightDial.Show(_session.Clock);
            IReadOnlyList<string> rows = GameClockFormatter.FormatHudClockRows(_session.Clock, _session.DayPhaseSchedule);
            _clockDayLabel.text = rows[0];
            _clockTimeLabel.text = rows[1];
            _clockPhaseLabel.text = rows[2];
        }

        private void RefreshExplorationProgress(ExplorationProgress progress)
        {
            _explorationProgressLabel.text = $"Explored {progress.WholePercent}%";
        }

        private static void SizeHandsGrid(VisualElement handsGrid, float rowHeight)
        {
            // Before its first layout pass an element's height is NaN; the geometry event sizes it once known.
            if (!float.IsNaN(rowHeight))
            {
                handsGrid.style.width = FixedGridLayoutCalculator.WidthForHeight(InventoryGridShapes.Hands, rowHeight);
            }
        }

        // Nothing that takes time can happen while a find waits for room on the ground.
        private void RefreshTimeTakingActions()
        {
            bool available = !_session.HasPendingGroundOverflow;
            _exploreButton.SetEnabled(available);
            _waitButton.SetEnabled(available);
            _restButton.SetEnabled(available);
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

        // Every window behind the nav bar is unbuilt at the wireframe stage (Docs/Design/UI/ScreenLayout.md).
        private void BindNavIconAndTooltip(Button button, NavigationBarEntry entry, GridTooltipView tooltip)
        {
            button.Add(new PixelIconElement(_pixelArtTextures.NavigationIcon(entry.IconId)));
            string[] details = entry.AvailabilityNote.Length > 0 ? new[] { entry.AvailabilityNote } : Array.Empty<string>();
            var content = new ThingTooltipContent(entry.Name, string.Empty, entry.Description, details);
            button.RegisterCallback<PointerEnterEvent>(_ => tooltip.Show(content, button.worldBound));
            button.RegisterCallback<PointerLeaveEvent>(_ => tooltip.Hide());
            button.clicked += tooltip.Hide;
        }

        private void BindNotBuiltNavButton(Button button, NavigationBarEntry entry)
        {
            button.clicked += () =>
            {
                _lastChosenNavButton = button;
                _notBuiltNoticeText.text = $"{entry.Name} isn't built yet.";
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
            InventoryGridView[] grids = { _zoneFeaturesView, _groundPoolView, _inventoryView, _handsView };
            if (Array.Exists(grids, grid => grid.IsDragging))
            {
                // Escape during a drag only cancels the drag (Docs/Design/UI/DragAndDrop.md, Dragging).
                Array.ForEach(grids, grid => grid.CancelDrag());
            }
            else if (Array.Exists(grids, grid => grid.HasSelection))
            {
                Array.ForEach(grids, grid => grid.ClearSelection());
            }
            else if (_groundOverflowPrompt.IsChoosingDiscard)
            {
                _groundOverflowPrompt.ReturnToQuestion();
            }
            else if (_groundOverflowPrompt.IsAsking)
            {
                // The question must be answered; Escape does not dismiss it (Docs/Design/UI/InventoryGrids.md).
            }
            else if (_notBuiltNotice.style.display != DisplayStyle.None)
            {
                HideNotBuiltNotice();
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
