using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    /// <summary>
    /// Abstract class enforcing the implementation of key combinatorial methods.
    /// </summary>
    public abstract class CombinatorialCore : CombinatorialBase
    {
        public CombinatorialCore(Group group, IComponent rawComponent, IMessageProvider messageProvider) : base(group, rawComponent, messageProvider)
        {
        }

        public override abstract long? Cardinality { get; }

        protected abstract override List<IMessage> GetPreGroupMessage();

        protected abstract override List<IMessage> GetPostGroupMessage();
    }
}
