using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeTests
{
    public class TestContext : CsvGnome.IContext
    {
        public long N { get; set; } = 1;
    }
}
