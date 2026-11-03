using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Content
{
    /// <summary>
    /// The definitions of one kind (items or zone features) made from the content files so far. It remembers every id
    /// a file declares, even for a record with a problem, so a zone naming that id is not also reported as naming an
    /// unknown one.
    /// </summary>
    internal sealed class ContentDefinitionRegistry<TDefinition>
        where TDefinition : class
    {
        private readonly HashSet<string> _declaredIds = new HashSet<string>(StringComparer.Ordinal);
        private readonly Dictionary<string, TDefinition> _validById = new Dictionary<string, TDefinition>(StringComparer.Ordinal);
        private readonly List<TDefinition> _validInFileOrder = new List<TDefinition>();

        /// <summary>The valid definitions, in file order.</summary>
        public IReadOnlyList<TDefinition> Definitions => _validInFileOrder;

        /// <summary>Records that a file declares the id. Returns false when an earlier record already did.</summary>
        public bool TryDeclare(string id) => _declaredIds.Add(id);

        /// <summary>Adds the definition made from a valid record.</summary>
        public void AddValid(string id, TDefinition definition)
        {
            _validById.Add(id, definition);
            _validInFileOrder.Add(definition);
        }

        /// <summary>Whether some record declares the id, valid or not.</summary>
        public bool IsDeclared(string id) => _declaredIds.Contains(id);

        /// <summary>Finds the definition made from the valid record with the id.</summary>
        public bool TryGetValid(string id, out TDefinition definition) => _validById.TryGetValue(id, out definition);
    }
}
