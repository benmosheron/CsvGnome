using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    static class Defaults
    {
        public static List<IField> GetFields()
        {
            var fields = new List<IField>();

            fields.Add(new ConstantField("Test", "MEOW"));
            fields.Add(new ConstantField("Testingtons", "MEWO!"));

            return fields;
        }
    }
}
