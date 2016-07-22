using System;
using System.Collections.Generic;
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
            : new Message($"{valueArray.Length} items", Program.SpecialColour),
            new Message("}"),
        };

        public bool Equals(IComponent x)
        {
            if (x == null) return false;
            var c = x as ArrayCycleComponent;
            if (c == null) return false;
            if (Command != c.Command) return false;
            return true;
        }

        #endregion IComponent

        #region ICombinatorial

        public override long? Cardinality => valueArray.Length;

        #endregion

        private readonly string[] valueArray;

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
            this.valueArray = new string[rawComponent.ValueArray.Count];
            rawComponent.ValueArray.CopyTo(this.valueArray, 0);
            if (this.valueArray == null || this.valueArray.Length == 0) this.valueArray = new string[] { String.Empty };
        }
    }
}
