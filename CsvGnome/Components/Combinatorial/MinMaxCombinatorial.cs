using System.Collections.Generic;

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
            return Message.NewSpecial($"[{RawMinMaxComponent.Min}=>{RawMinMaxComponent.Max},{RawMinMaxComponent.Increment} ").ToList();
        }

        protected override List<Message> GetPostGroupMessage()
        {
            return Message.NewSpecial("]").ToList();
        }
    }
}
