using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeScript
{
    public class InvalidFunctionException : Exception
    {
        public string Function;
        public string Language;
        public InvalidFunctionException(string language, string invalidFunction)
            :base($"Invalid function [{invalidFunction}] for language [{language}].")
        {
            Function = invalidFunction;
            Language = language;
        }
    }
}
