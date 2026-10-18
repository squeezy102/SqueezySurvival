using System;

namespace SqueezySurvival.Core.Items
{
    /// <summary>Validation shared by the countable definitions.</summary>
    internal static class CountableThingDefinitionText
    {
        public static string Require(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Cannot be empty.", parameterName);
            }

            return value;
        }
    }
}
