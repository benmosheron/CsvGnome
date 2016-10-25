using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class MinMaxCombinatorial : CombinatorialCore, IComponent
    {
        public MinMaxCombinatorial(Group group, MinMaxComponent rawComponent, IMessageProvider messageProvider = null)
            : base(group, rawComponent, messageProvider)
        {
        }

        public static MinMaxCombinatorial Make(Group group, MinMaxComponent rawComponent, IMessageProvider messageProvider)
        {
            return new MinMaxCombinatorial(group, rawComponent, messageProvider);
        }

        public string Command => $"[{RawMinMaxComponent.Min}=>{RawMinMaxComponent.Max},{RawMinMaxComponent.Increment} {GetGroupString()}]";

        private MinMaxComponent RawMinMaxComponent => RawComponent as MinMaxComponent;

        public override long? Cardinality
        {
            get
            {
                return RawMinMaxComponent.Cardinality;
            }
        }

        protected override List<IMessage> GetPreGroupMessage()
        {
            return MessageProvider.NewSpecial($"[{RawMinMaxComponent.Min}=>{RawMinMaxComponent.Max},{RawMinMaxComponent.Increment} ").ToList();
        }

        protected override List<IMessage> GetPostGroupMessage()
        {
            return MessageProvider.NewSpecial("]").ToList();
        }
    }
}
