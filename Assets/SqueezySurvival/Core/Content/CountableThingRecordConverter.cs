using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Content.Records;

namespace SqueezySurvival.Core.Content
{
    /// <summary>
    /// The checks items and zone features share (an id that is present and unique, and their text), and the loop over
    /// a file's records. What only one kind has is checked and built by the caller.
    /// </summary>
    internal static class CountableThingRecordConverter
    {
        /// <param name="fileName">The file the records come from, for problems.</param>
        /// <param name="kind">What a record is called in problems: "item".</param>
        /// <param name="records">The file's records, in file order.</param>
        /// <param name="problems">Where problems are collected.</param>
        /// <param name="checkOwnFields">Reports problems in the fields only this kind of record has.</param>
        /// <param name="createDefinition">Makes the definition of a record that has no problems.</param>
        public static ContentDefinitionRegistry<TDefinition> Convert<TRecord, TDefinition>(
            string fileName,
            string kind,
            IReadOnlyList<TRecord> records,
            GameContentProblems problems,
            Action<TRecord, string> checkOwnFields,
            Func<TRecord, TDefinition> createDefinition)
            where TRecord : CountableThingRecord
            where TDefinition : class
        {
            var registry = new ContentDefinitionRegistry<TDefinition>();
            for (int position = 0; position < records.Count; position++)
            {
                TRecord record = records[position];
                string subject = ContentRecordChecks.Subject(kind, record?.Id, position);
                if (record == null)
                {
                    problems.Add(fileName, subject, "the entry is empty");
                    continue;
                }

                int problemsBefore = problems.Count;
                if (string.IsNullOrWhiteSpace(record.Id))
                {
                    problems.Add(fileName, subject, "id is empty");
                }
                else if (!registry.TryDeclare(record.Id))
                {
                    problems.Add(fileName, subject, $"id is already used by an earlier {kind}");
                }

                ContentRecordChecks.RequireText(problems, fileName, subject, "name", record.Name);
                ContentRecordChecks.RequireText(problems, fileName, subject, "singular phrase", record.SingularPhrase);
                ContentRecordChecks.RequireText(problems, fileName, subject, "plural name", record.PluralName);
                ContentRecordChecks.RequireText(problems, fileName, subject, "description", record.Description);
                checkOwnFields(record, subject);

                if (problems.Count == problemsBefore)
                {
                    registry.AddValid(record.Id, createDefinition(record));
                }
            }

            return registry;
        }
    }
}
