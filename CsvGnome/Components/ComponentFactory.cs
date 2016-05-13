using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class ComponentFactory
    {
        private const string IncrementingPattern = @"\[\-*\d*\+\+\-*\d*\]";
        private Regex IncrementingRegex = new Regex(IncrementingPattern);

        public IComponent Create(string prototype)
        {
            // e.g.
            // [++]
            // [1++2]
            // [-99++-109]
            if (IncrementingRegex.IsMatch(prototype))
            {
                // remove []
                string protoInc = prototype.Substring(1, prototype.Length - 2);
                string[] tokens = protoInc.Split(new string[] { "++" },StringSplitOptions.None);
                int start;
                int increment;
                if (!int.TryParse(tokens[0], out start)) start = IncrementingComponent.DefaultStart;
                if (!int.TryParse(tokens[1], out increment)) increment = IncrementingComponent.DefaultIncrement;

                return new IncrementingComponent(start, increment);
            }
            else if(prototype == Program.DateComponentString)
            {
                return new DateComponent();
            }
            else
            {
                return new TextComponent(prototype);
            }
        }
    }
}
