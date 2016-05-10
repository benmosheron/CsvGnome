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
            fields.Add(new IncrementingField("pppp", "omg_"));
            fields.Add(new IncrementingField("ppp", "omg_", 98));


            return fields;
        }
    }
}
