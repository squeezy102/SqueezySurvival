namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>
    /// Remembers the last drop outcome worked out for a drag, so a drag held still does not work out and word the same
    /// outcome again on every check. It holds one outcome: a drag only ever shows the latest.
    /// </summary>
    public sealed class DropOutcomeMemo
    {
        private bool _hasOutcome;
        private DropOutcomeKey _key;
        private GridDropOutcome _outcome;

        /// <summary>The remembered outcome when it was worked out for exactly <paramref name="key"/>.</summary>
        public bool TryGet(DropOutcomeKey key, out GridDropOutcome outcome)
        {
            outcome = _outcome;
            return _hasOutcome && _key.Equals(key);
        }

        /// <summary>Remembers <paramref name="outcome"/> as the one for <paramref name="key"/>, forgetting any earlier one.</summary>
        public void Store(DropOutcomeKey key, GridDropOutcome outcome)
        {
            _key = key;
            _outcome = outcome;
            _hasOutcome = true;
        }

        /// <summary>Forgets the remembered outcome, as when a drag ends.</summary>
        public void Clear()
        {
            _hasOutcome = false;
            _outcome = null;
        }
    }
}
