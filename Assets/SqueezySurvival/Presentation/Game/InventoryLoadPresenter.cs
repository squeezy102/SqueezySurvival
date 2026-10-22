using System;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>Shows how much the survivor carries in the inventory header (Docs/Design/Systems/Inventory.md, Load).</summary>
    public sealed class InventoryLoadPresenter : IDisposable
    {
        private readonly SurvivorInventoryService _survivorInventory;
        private readonly Label _loadLabel;

        /// <summary>Binds the load label in <paramref name="root"/> to what the survivor carries.</summary>
        public InventoryLoadPresenter(VisualElement root, SurvivorInventoryService survivorInventory)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            _survivorInventory = survivorInventory ?? throw new ArgumentNullException(nameof(survivorInventory));
            _loadLabel = root.Q<Label>("inventory-load");
            _survivorInventory.Inventory.Changed += OnCarriedThingsChanged;
            _survivorInventory.Hands.Changed += OnCarriedThingsChanged;
            Refresh();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _survivorInventory.Inventory.Changed -= OnCarriedThingsChanged;
            _survivorInventory.Hands.Changed -= OnCarriedThingsChanged;
        }

        // The load is a total of everything carried, so any change recomputes it whatever slots changed.
        private void OnCarriedThingsChanged(object sender, CountedCollectionChange change) => Refresh();

        private void Refresh()
        {
            _loadLabel.text = SurvivorLoadTextFormatter.Format(_survivorInventory.Load);
        }
    }
}
