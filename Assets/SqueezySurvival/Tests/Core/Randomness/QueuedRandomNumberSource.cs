using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Randomness;

namespace SqueezySurvival.Core.Tests.Randomness
{
    /// <summary>Returns queued numbers in order, checking each is inside the requested range.</summary>
    public sealed class QueuedRandomNumberSource : IRandomNumberSource
    {
        private readonly Queue<int> _numbers;

        public QueuedRandomNumberSource(params int[] numbers)
        {
            _numbers = new Queue<int>(numbers);
        }

        public int RemainingCount => _numbers.Count;

        public int NextInt(int minimumInclusive, int maximumExclusive)
        {
            if (_numbers.Count == 0)
            {
                throw new InvalidOperationException("No queued random numbers left.");
            }

            int number = _numbers.Dequeue();
            if (number < minimumInclusive || number >= maximumExclusive)
            {
                throw new InvalidOperationException($"Queued {number} is outside [{minimumInclusive}, {maximumExclusive}).");
            }

            return number;
        }
    }
}
