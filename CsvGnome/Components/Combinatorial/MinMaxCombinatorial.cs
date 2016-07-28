using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class MinMaxCombinatorial : CombinatorialCore, IComponent
    {
        public MinMaxCombinatorial(Group group, MinMaxComponent rawComponent)
            : base(group, rawComponent)
        {
        }

        public static MinMaxCombinatorial Make(Group group, MinMaxComponent rawComponent)
        {
            return new MinMaxCombinatorial(group, rawComponent);
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

        protected override List<Message> GetPreGroupMessage()
        {
            List<Message> m = new List<Message>();
            m.Add(new Message("[", Program.SpecialColour));
            m.Add(new Message($"{RawMinMaxComponent.Min}"));
            m.Add(new Message("=>", Program.SpecialColour));
            m.Add(new Message($"{RawMinMaxComponent.Max}"));
            m.Add(new Message($",{RawMinMaxComponent.Increment} "));
            return m;
        }

        protected override List<Message> GetPostGroupMessage()
        {
            return new Message("]", Program.SpecialColour).ToList();
        }
    }
}
