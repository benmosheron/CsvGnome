using CsvGnome;
using CsvGnome.Components.Combinatorial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeTests
{
    public static class Utilties
    {
        public static void InterpreterTestInit(
            out FieldBrain fieldBrain,
            out Interpreter interpreter)
        {
            Reporter reporter = new Reporter();
            MinMaxInfoCache minMaxInfoCache = new MinMaxInfoCache();
            Cache cache = new Cache();
            Factory combinatorialFactory = new Factory(cache);
            fieldBrain = new FieldBrain();
            interpreter = new Interpreter(fieldBrain, reporter, combinatorialFactory, minMaxInfoCache);
        }

        public static void InterpreterTestInit(
            out FieldBrain fieldBrain,
            out MinMaxInfoCache minMaxInfoCache,
            out Interpreter interpreter)
        {
            Reporter reporter = new Reporter();
            minMaxInfoCache = new MinMaxInfoCache();
            Cache cache = new Cache();
            Factory combinatorialFactory = new Factory(cache);
            fieldBrain = new FieldBrain();
            interpreter = new Interpreter(fieldBrain, reporter, combinatorialFactory, minMaxInfoCache);
        }
    }
}
