using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class AlphabetCombinatorial : CombinatorialCore, IComponent
    {
        public AlphabetCombinatorial(Group group, AlphabetComponent rawComponent) : base(group, rawComponent)
        {
        }

        public AlphabetComponent RawAlphabetComponent => RawComponent as AlphabetComponent;

        public override long? Cardinality => RawAlphabetComponent.Values.Length;

        public string Command => $"[{RawAlphabetComponent.Start}=>{RawAlphabetComponent.End} {GetGroupString()}]";

        protected override List<Message> GetPreGroupMessage()
        {
            return new List<Message>
            {
                Message.NewSpecial("["),
                new Message(RawAlphabetComponent.Start.ToString()),
                Message.NewSpecial("=>"),
                new Message(RawAlphabetComponent.End.ToString()),
                Message.NewSpecial(" ")
            };
        }

        protected override List<Message> GetPostGroupMessage()
        {
            return new List<Message>
            {
                Message.NewSpecial("]")
            };
        }
    }
}
