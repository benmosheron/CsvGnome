using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class CombinatorialGroupException : Exception
    {
        public CombinatorialGroupException(string message)
            : base(message)
        {

        }
    }
}
