using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeScript
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
