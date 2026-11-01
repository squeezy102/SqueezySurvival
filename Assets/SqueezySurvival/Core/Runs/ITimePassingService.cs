using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>Wait and Rest: actions whose only purpose is to pass time (Docs/Design/Systems/Time.md, Passing time deliberately).</summary>
    public interface ITimePassingService
    {
        /// <summary>Whether time can be passed now.</summary>
        bool CanPassTime { get; }

        /// <summary>Performs a time-passing action and logs it.</summary>
        void PassTime(TimePassingAction action);
    }
}
