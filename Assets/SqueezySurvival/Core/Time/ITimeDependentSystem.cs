namespace SqueezySurvival.Core.Time
{
    /// <summary>
    /// A system that changes as time passes, such as survival stats, weather or research. The game time service tells
    /// each one, in a fixed order, after every action (Docs/Engineering/Architecture.md, Time: one path).
    /// </summary>
    public interface ITimeDependentSystem
    {
        /// <summary>Applies the effects of the time that passed. Must not advance time itself.</summary>
        void ApplyTimePassed(GameTimeAdvance advance);
    }
}
