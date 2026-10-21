using System;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Navigation;
using SqueezySurvival.Presentation.Icons;
using SqueezySurvival.Presentation.PixelArt;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// The nav bar: each button's icon and tooltip; Options opens Options, and every gameplay window, unbuilt at the
    /// wireframe stage, says so (Docs/Design/UI/ScreenLayout.md, Nav bar).
    /// </summary>
    public sealed class NavigationBarPresenter
    {
        private readonly PixelArtTextureCache _textures;
        private readonly GridTooltipView _tooltip;
        private readonly GameScreenOverlayPresenter _overlays;

        /// <summary>Binds the nav bar buttons in <paramref name="root"/>.</summary>
        public NavigationBarPresenter(VisualElement root, PixelArtTextureCache textures, GridTooltipView tooltip, GameScreenOverlayPresenter overlays)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            _textures = textures ?? throw new ArgumentNullException(nameof(textures));
            _tooltip = tooltip ?? throw new ArgumentNullException(nameof(tooltip));
            _overlays = overlays ?? throw new ArgumentNullException(nameof(overlays));

            Button optionsButton = root.Q<Button>("nav-options-button");
            BindIconAndTooltip(optionsButton, NavigationBarEntries.Options);
            optionsButton.clicked += () => _overlays.OpenOptions(optionsButton);
            foreach (NavigationBarEntry entry in NavigationBarEntries.GameplayWindows)
            {
                Button button = root.Q<Button>($"nav-{entry.Key}-button");
                BindIconAndTooltip(button, entry);
                button.clicked += () => _overlays.ShowNotBuiltNotice(entry, button);
            }
        }

        private void BindIconAndTooltip(Button button, NavigationBarEntry entry)
        {
            button.Add(new PixelIconElement(_textures.NavigationIcon(entry.IconId)));
            string[] details = entry.AvailabilityNote.Length > 0 ? new[] { entry.AvailabilityNote } : Array.Empty<string>();
            var content = new ThingTooltipContent(entry.Name, string.Empty, entry.Description, details);
            button.RegisterCallback<PointerEnterEvent>(_ => _tooltip.Show(content, button.worldBound));
            button.RegisterCallback<PointerLeaveEvent>(_ => _tooltip.Hide());
            button.clicked += _tooltip.Hide;
        }
    }
}
