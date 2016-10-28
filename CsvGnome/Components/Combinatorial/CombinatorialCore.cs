using System.Collections.Generic;

namespace CsvGnome.Components.Combinatorial
{
    /// <summary>
    /// Abstract class enforcing the implementation of key combinatorial methods.
    /// </summary>
    public abstract class CombinatorialCore : CombinatorialBase
    {
        public CombinatorialCore(Group group, IComponent rawComponent) : base(group, rawComponent)
        {
        }

        public override abstract long? Cardinality { get; }

        protected abstract override List<Message> GetPreGroupMessage();

        protected abstract override List<Message> GetPostGroupMessage();
    }
}
