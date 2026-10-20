namespace SqueezySurvival.Core.EventLog
{
    /// <summary>How an event log entry is marked (Docs/Design/UI/HUD.md, Event log).</summary>
    public enum EventLogSeverity
    {
        /// <summary>An ordinary entry.</summary>
        Normal,

        /// <summary>A warning: shown in red with a warning icon, and worded as a warning, so color is never the only signal.</summary>
        Warning,
    }
}
