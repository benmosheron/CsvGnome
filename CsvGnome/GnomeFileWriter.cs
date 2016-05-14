using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class GnomeFileWriter
    {
        Interpreter Interpreter;
        Reporter Reporter;
        Dictionary<string, string> gnomeFileCache;

        public GnomeFileWriter(Interpreter interpreter, Reporter reporter, Dictionary<string, string> gnomeFileCache)
        {
            Interpreter = interpreter;
            Reporter = reporter;
            this.gnomeFileCache = gnomeFileCache;
        }

    }
}
