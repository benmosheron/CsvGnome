using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class ArrayCycleCombinatorial : CombinatorialBase, IComponent
    {
        #region IComponent

        public string Command
        {
            get
            {
                return CommandInitString + "{" + valueArray.Aggregate((t, n) => $"{t},{n}") + "}";
            }
        }

        public List<Message> Summary => new List<Message>()
        {
            new Message(CommandInitString, Program.SpecialColour),
            new Message("{"),
            Program.ReportArrayContents
            ? new Message(valueArray.Aggregate((t, n) => $"{t},{n}"))
            : new Message($"{valueArray.Count} items", Program.SpecialColour),
            new Message("}"),
        };

        public bool Equals(IComponent x)
        {
            return base.Equals(x);
        }

        #endregion IComponent

        #region ICombinatorial

        public override long? Cardinality => valueArray.Count;

        #endregion

        private ArrayCycleComponent RawArrayCycleComponent => RawComponent as ArrayCycleComponent;

        private ReadOnlyCollection<string> valueArray => RawArrayCycleComponent.ValueArray;

        private string CommandInitString
        {
            get
            {
                string groupIdString = $"#{Group.Id}/{Group.RankOf(this)}";
                return Program.CycleCombinatorialString.Replace("#", groupIdString);
            }
        }

        /// <summary>
        /// Do not use this! Use the Factory class, which will manage the cache.
        /// </summary>
        public ArrayCycleCombinatorial(
            Group group,
            ArrayCycleComponent rawComponent)
            :base(group, rawComponent)
        {

        }
    }
}
