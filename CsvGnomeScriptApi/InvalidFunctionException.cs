using System;

namespace CsvGnomeScriptApi
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
