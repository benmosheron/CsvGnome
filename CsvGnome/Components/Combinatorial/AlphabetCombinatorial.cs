using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class AlphabetCombinatorial : CombinatorialCore, IComponent
    {
        public AlphabetCombinatorial(Group group, AlphabetComponent rawComponent, IMessageProvider messageProvider = null) : base(group, rawComponent, messageProvider)
        {
        }

        /// <summary>
        /// Static "constructor" allows us to create these from a generic factory method.
        /// </summary>
        public static AlphabetCombinatorial Make(Group group, AlphabetComponent rawComponent, IMessageProvider messageProvider)
        {
            return new AlphabetCombinatorial(group, rawComponent, messageProvider);
        }

        public AlphabetComponent RawAlphabetComponent => RawComponent as AlphabetComponent;

        public override long? Cardinality => RawAlphabetComponent.Values.Length;

        public string Command => $"[{RawAlphabetComponent.Start}=>{RawAlphabetComponent.End} {GetGroupString()}]";

        protected override List<IMessage> GetPreGroupMessage()
        {
            return new List<IMessage>
            {
                MessageProvider.NewSpecial("["),
                MessageProvider.New(RawAlphabetComponent.Start.ToString()),
                MessageProvider.NewSpecial("=>"),
                MessageProvider.New(RawAlphabetComponent.End.ToString()),
                MessageProvider.NewSpecial(" ")
            };
        }

        protected override List<IMessage> GetPostGroupMessage()
        {
            return new List<IMessage>
            {
                MessageProvider.NewSpecial("]")
            };
        }
    }
}
