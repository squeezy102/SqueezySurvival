using System;
using System.Globalization;

namespace SqueezySurvival.Core.Content
{
    /// <summary>Checks and wording shared by the record converters.</summary>
    internal static class ContentRecordChecks
    {
        /// <summary>Names a record in a problem: "item 'stone'", or its position in the file when it has no id.</summary>
        public static string Subject(string kind, string id, int zeroBasedPosition) =>
            string.IsNullOrWhiteSpace(id) ? $"{kind} at position {zeroBasedPosition + 1}" : $"{kind} '{id}'";

        /// <summary>Reports text that is missing or only whitespace.</summary>
        public static void RequireText(GameContentProblems problems, string fileName, string subject, string label, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                problems.Add(fileName, subject, $"{label} is empty");
            }
        }

        /// <summary>Reports an enum that the file left out, or that holds a number no name stands for.</summary>
        public static void RequireKnownValue<TEnum>(GameContentProblems problems, string fileName, string subject, string label, TEnum? value)
            where TEnum : struct
        {
            if (value == null)
            {
                problems.Add(fileName, subject, $"{label} is missing");
            }
            else if (!Enum.IsDefined(typeof(TEnum), value.Value))
            {
                problems.Add(fileName, subject, $"{label} is not a known value");
            }
        }

        /// <summary>Writes a number the same way on every computer, whatever its language settings.</summary>
        public static string Number(IFormattable value) => value.ToString(null, CultureInfo.InvariantCulture);
    }
}
