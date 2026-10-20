namespace SqueezySurvival.Core.Time
{
    /// <summary>
    /// The one path that advances game time (Docs/Design/Systems/Time.md, Implementation notes). Only the run's
    /// commands hold this; everything else sees <see cref="IGameTime"/>.
    /// </summary>
    public interface IGameTimeAdvancer : IGameTime
    {
        /// <summary>Advances the clock by an action's duration and tells every time-dependent system, in order.</summary>
        GameTimeAdvance Advance(ActionDuration duration);
    }
}
