using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class ComponentCreationException : Exception
    {
        public ComponentCreationException(string message, Exception inner) : base(message, inner) { }
    }
}
