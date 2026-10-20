namespace SqueezySurvival.Core.Survivor
{
    /// <summary>How the survivor's load affects them (Docs/Design/Systems/Inventory.md, Load).</summary>
    public enum SurvivorLoadState
    {
        /// <summary>Below 75%: no effect.</summary>
        Normal,

        /// <summary>75% to just under 100%: actions cost more stamina.</summary>
        Overburdened,

        /// <summary>100% and over: the survivor can't move to explore or travel.</summary>
        Immobile,
    }
}
