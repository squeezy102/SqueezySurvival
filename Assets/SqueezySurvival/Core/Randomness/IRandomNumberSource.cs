namespace SqueezySurvival.Core.Randomness
{
    /// <summary>
    /// Supplies random numbers to game systems. Systems never create their own random generator, so tests can supply
    /// a fixed sequence and every random outcome is reproducible.
    /// </summary>
    public interface IRandomNumberSource
    {
        /// <summary>Returns a random whole number from <paramref name="minimumInclusive"/> up to, but not including, <paramref name="maximumExclusive"/>.</summary>
        int NextInt(int minimumInclusive, int maximumExclusive);
    }
}
