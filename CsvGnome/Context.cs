using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class Context : IContext
    {
        long n = 10;
        public long N
        {
            get
            {
                return n;
            }
            set
            {
                if (value >= 0) n = value;
                else n = 0;
            }
        }
    }
}
