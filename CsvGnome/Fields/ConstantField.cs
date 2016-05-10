using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    class ConstantField : IField
    {
        public string Name { get; }

        public string Summary { get; }

        public string GetValue(int i) => Summary;

        public ConstantField(string name, string value)
        {
            Name = name;
            Summary = value;
        }
    }
}
