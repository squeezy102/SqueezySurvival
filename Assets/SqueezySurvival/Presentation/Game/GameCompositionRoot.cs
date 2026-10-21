using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Controls;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Exploration;
using SqueezySurvival.Core.GameScreen;
using SqueezySurvival.Core.InventoryGrids;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Randomness;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;
using SqueezySurvival.Presentation.PixelArt;
using SqueezySurvival.Presentation.Session;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// The game scene's one composition root: starts the run handed off by the New Game screen, builds every service,
    /// view and presenter the scene needs, hands each its collaborators, and disposes them with the scene
    /// (Docs/Engineering/Architecture.md, One composition root per scene). It presents nothing itself. Without a
    /// handed-off run (for example, Play pressed with this scene open) it returns to the main menu
    /// (Docs/Design/UI/ScreenLayout.md, Wireframe stage).
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public sealed class GameCompositionRoot : MonoBehaviour
    {
        private readonly List<IDisposable> _ownedInCreationOrder = new List<IDisposable>();

        // Start runs after every OnEnable, so the UIDocument has built its visual tree by then.
        private void Start()
        {
            if (!GameSessionHost.Current.RunLaunchHandoff.TryTake(out NewRunConfiguration configuration))
            {
                SceneManager.LoadScene(SceneNames.MainMenu);
                return;
            }

            Compose(CreateSession(configuration), GetComponent<UIDocument>().rootVisualElement);
        }

        private void OnDestroy()
        {
            for (int index = _ownedInCreationOrder.Count - 1; index >= 0; index--)
            {
                _ownedInCreationOrder[index].Dispose();
            }

            _ownedInCreationOrder.Clear();
        }

        private static RunSession CreateSession(NewRunConfiguration configuration)
        {
            // A new seed each run. Saving the generator's state with the run belongs to saving (#2).
            var randomNumberSource = new SystemRandomNumberSource(Environment.TickCount);

            // Time-dependent systems in the order Architecture.md documents (Time: one path). None exist yet.
            var gameTimeAdvancer = new GameTimeAdvancer(GameClock.RunStart, Array.Empty<ITimeDependentSystem>());
            return new RunSession(
                configuration,
                DayPhaseSchedule.Default,
                gameTimeAdvancer,
                PlaceholderBeachZoneContent.Beach,
                new ZoneExplorer(ExplorationRules.Wireframe, randomNumberSource));
        }

        private void Compose(RunSession session, VisualElement root)
        {
            PixelArtTextureCache textures = Own(new PixelArtTextureCache());
            var skin = new PixelArtInterfaceSkin(textures);
            var tooltip = new GridTooltipView(root.Q<VisualElement>("grid-tooltip"), textures);
            var dropOutcomeResolver = new GridDropOutcomeResolver(new NoThingCombinations());
            VisualElement dragLayer = root.Q<VisualElement>("game-screen-root");
            Zone zone = session.CurrentZone;

            InventoryGridView CreateGrid(
                FixedGridShape shape,
                FixedGridHorizontalAlignment alignment,
                IInventoryGridContents contents,
                string gridName,
                Func<int, Texture2D> emptySlotGhost = null) =>
                Own(new InventoryGridView(shape, alignment, contents, root.Q<VisualElement>(gridName), tooltip, dragLayer, skin, emptySlotGhost));

            var groundContents = new CountedCollectionInventoryGridContents<ItemDefinition>(zone.GroundPool, dropOutcomeResolver);
            var inventoryContents = new CountedCollectionInventoryGridContents<ItemDefinition>(session.Inventory, dropOutcomeResolver);
            InventoryGridView zoneFeaturesView = CreateGrid(
                InventoryGridShapes.ZoneFeatures,
                FixedGridHorizontalAlignment.Center,
                new CountedCollectionInventoryGridContents<ZoneFeatureDefinition>(zone.Features, dropOutcomeResolver),
                "zone-features-grid");
            InventoryGridView groundView = CreateGrid(InventoryGridShapes.Ground, FixedGridHorizontalAlignment.Center, groundContents, "ground-pool-grid");
            InventoryGridView inventoryView = CreateGrid(InventoryGridShapes.Inventory, FixedGridHorizontalAlignment.Left, inventoryContents, "inventory-grid");
            InventoryGridView handsView = CreateGrid(
                InventoryGridShapes.Hands,
                FixedGridHorizontalAlignment.Left,
                new CountedCollectionInventoryGridContents<ItemDefinition>(session.Hands, dropOutcomeResolver),
                "hands-grid",
                slot => textures.HandGhost(HandSlots.HandAt(slot)));
            new HandsGridSizer(root.Q<VisualElement>("hands-grid"));
            new InventoryGridSortController(groundContents, root.Q<Button>("ground-pool-sort-button"), root.Q<Button>("ground-pool-sort-menu-button"));
            new InventoryGridSortController(inventoryContents, root.Q<Button>("inventory-sort-button"), root.Q<Button>("inventory-sort-menu-button"));

            var itemMoves = new ItemPanelMoveCoordinator(session, new KeyboardMoveModifierReader(MoveModifierBindings.Default));
            itemMoves.Register(ItemPanel.Ground, groundView);
            itemMoves.Register(ItemPanel.Inventory, inventoryView);
            GroundOverflowPromptController groundOverflowPrompt = Own(new GroundOverflowPromptController(
                session, groundView, root.Q<VisualElement>("ground-overflow-prompt"), root.Q<Label>("ground-discard-hint")));

            TimeTakingActionsPresenter timeTakingActions = Own(new TimeTakingActionsPresenter(root, session));
            Own(new HudClockPresenter(root, session.GameTime, session.DayPhaseSchedule));
            Own(new ZoneBarPresenter(root, zone));
            Own(new InventoryLoadPresenter(root, session));
            Own(new EventLogView(root.Q<VisualElement>("event-log-panel"), session.EventLog, textures, new UnreadWarningTracker()));

            var overlays = new GameScreenOverlayStack();
            GameScreenOverlayPresenter overlayPresenter = Own(new GameScreenOverlayPresenter(root, overlays, timeTakingActions.FocusDefault));
            new NavigationBarPresenter(root, textures, tooltip, overlayPresenter);
            Own(new GameScreenEscapeKeyPresenter(
                new GameScreenEscapeResolver(overlays),
                new[] { zoneFeaturesView, groundView, inventoryView, handsView },
                groundOverflowPrompt));

            skin.ApplyTo(root);
            skin.ApplyBackdrop(dragLayer);

            // Focus must be requested a frame after layout for UI Toolkit to apply it.
            root.schedule.Execute(timeTakingActions.FocusDefault);
        }

        private T Own<T>(T disposable)
            where T : IDisposable
        {
            _ownedInCreationOrder.Add(disposable);
            return disposable;
        }
    }
}
