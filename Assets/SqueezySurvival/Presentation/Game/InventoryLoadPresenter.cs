using System;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>Shows how much the survivor carries in the inventory header (Docs/Design/Systems/Inventory.md, Load).</summary>
    public sealed class InventoryLoadPresenter : IDisposable
    {
        private readonly RunSession _session;
        private readonly Label _loadLabel;

        /// <summary>Binds the load label in <paramref name="root"/> to what the survivor carries.</summary>
        public InventoryLoadPresenter(VisualElement root, RunSession session)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            _session = session ?? throw new ArgumentNullException(nameof(session));
            _loadLabel = root.Q<Label>("inventory-load");
            _session.Inventory.Changed += Refresh;
            _session.Hands.Changed += Refresh;
            Refresh();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _session.Inventory.Changed -= Refresh;
            _session.Hands.Changed -= Refresh;
        }

        private void Refresh()
        {
            _loadLabel.text = SurvivorLoadTextFormatter.Format(_session.Load);
        }
    }
}
