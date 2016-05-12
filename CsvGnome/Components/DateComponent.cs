using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    class DateComponent : IComponent
    {
        public string Summary => Program.DateComponentString;
        public string GetValue(int i)
        {
            throw new NotImplementedException();
        }
    }
}
