using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class DateComponent : IComponent
    {
        public string Command => Program.DateComponentString;
        public List<Message> Summary => new List<Message> { new Message(Program.DateComponentString, Program.SpecialColour) };
        public string GetValue(long i)
        {
            return Program.TimeAtWrite;
        }
        public bool Equals(IComponent x)
        {
            if (x == null) return false;
            var c = x as DateComponent;
            if (c == null) return false;
            if (GetValue(0) != c.GetValue(0)) return false;
            return true;
        }
    }
}
