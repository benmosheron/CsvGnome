using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Reads gnome files, which save the configuration of a set of fields for re-use.
    /// </summary>
    class GnomeFileReader
    {
        Interpreter Interpreter;

        public GnomeFileReader(Interpreter interpreter)
        {
            Interpreter = interpreter;
        }

        public void ReadGnomeFile(string pathAndFile)
        {
            try {
                using (StreamReader sr = new StreamReader(pathAndFile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Interpreter.Interpret(line);
                    }
                }
            }
            catch(FileNotFoundException ex)
            {
                
            }
        }
    }
}
