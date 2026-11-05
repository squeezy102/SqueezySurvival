using System;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Everything a dragged cell's drop outcome depends on. Two equal keys mean the same outcome, so a held drag need
    /// not work it out again. Immutable.
    /// </summary>
    public readonly struct DropOutcomeKey : IEquatable<DropOutcomeKey>
    {
        private readonly bool _overOtherPanel;
        private readonly ItemPanel _targetPanel;
        private readonly int _sourceSlot;
        private readonly GridDropTargetKind _targetKind;
        private readonly int _targetSlot;
        private readonly bool _attemptsCombine;
        private readonly ItemTransferQuantity _quantity;
        private readonly bool _canTransfer;
        private readonly long _contentsRevision;

        private DropOutcomeKey(
            bool overOtherPanel,
            ItemPanel targetPanel,
            int sourceSlot,
            GridDropTargetKind targetKind,
            int targetSlot,
            bool attemptsCombine,
            ItemTransferQuantity quantity,
            bool canTransfer,
            long contentsRevision)
        {
            _overOtherPanel = overOtherPanel;
            _targetPanel = targetPanel;
            _sourceSlot = sourceSlot;
            _targetKind = targetKind;
            _targetSlot = targetSlot;
            _attemptsCombine = attemptsCombine;
            _quantity = quantity;
            _canTransfer = canTransfer;
            _contentsRevision = contentsRevision;
        }

        /// <summary>A drag held over the dragged cell's own grid.</summary>
        public static DropOutcomeKey ForOwnGrid(int sourceSlot, GridDropTarget target, ItemTransferQuantity quantity, long contentsRevision)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            return new DropOutcomeKey(false, default, sourceSlot, target.Kind, target.Slot, target.AttemptsCombine, quantity, true, contentsRevision);
        }

        /// <summary>
        /// A drag held over <paramref name="targetPanel"/>, another panel than the dragged cell's own, with -1 for
        /// <paramref name="hoveredSlot"/> between slots.
        /// </summary>
        public static DropOutcomeKey ForOtherPanel(
            int sourceSlot, ItemPanel targetPanel, int hoveredSlot, ItemTransferQuantity quantity, bool canTransfer, long contentsRevision) =>
            new DropOutcomeKey(true, targetPanel, sourceSlot, GridDropTargetKind.Nowhere, hoveredSlot, false, quantity, canTransfer, contentsRevision);

        /// <inheritdoc />
        public bool Equals(DropOutcomeKey other) =>
            _overOtherPanel == other._overOtherPanel
            && _targetPanel == other._targetPanel
            && _sourceSlot == other._sourceSlot
            && _targetKind == other._targetKind
            && _targetSlot == other._targetSlot
            && _attemptsCombine == other._attemptsCombine
            && _quantity == other._quantity
            && _canTransfer == other._canTransfer
            && _contentsRevision == other._contentsRevision;

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is DropOutcomeKey other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() =>
            HashCode.Combine(_overOtherPanel, _targetPanel, _sourceSlot, _targetKind, _targetSlot, HashCode.Combine(_attemptsCombine, _quantity, _canTransfer, _contentsRevision));
    }
}
