using System;
using SqueezySurvival.Core.Zones;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>Shows the current zone's name and exploration progress (Docs/Design/UI/ScreenLayout.md, Zone bar).</summary>
    public sealed class ZoneBarPresenter : IDisposable
    {
        private readonly Zone _zone;
        private readonly Label _explorationProgressLabel;

        /// <summary>Binds the zone bar in <paramref name="root"/> to <paramref name="zone"/>.</summary>
        public ZoneBarPresenter(VisualElement root, Zone zone)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            _zone = zone ?? throw new ArgumentNullException(nameof(zone));
            root.Q<Label>("zone-name").text = zone.Name;
            _explorationProgressLabel = root.Q<Label>("zone-exploration-progress");
            _zone.ExplorationProgressChanged += Refresh;
            Refresh(zone.ExplorationProgress);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _zone.ExplorationProgressChanged -= Refresh;
        }

        private void Refresh(ExplorationProgress progress)
        {
            _explorationProgressLabel.text = $"Explored {progress.WholePercent}%";
        }
    }
}
