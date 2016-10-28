using System;

namespace CsvGnomeScriptApi
{
    public class InvalidLanguageException : Exception
    {
        public string Language;
        public InvalidLanguageException(string invalidLanguage)
            :base($"Invalid language [{invalidLanguage}].")
        {
            Language = invalidLanguage;
        }
    }
}
