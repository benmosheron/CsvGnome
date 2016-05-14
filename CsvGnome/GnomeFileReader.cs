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
    public class GnomeFileReader
    {
        Interpreter Interpreter;
        Reporter Reporter;
        GnomeFileCache GnomeFileCache;

        public GnomeFileReader(Interpreter interpreter, Reporter reporter, GnomeFileCache gnomeFileCache)
        {
            Interpreter = interpreter;
            Reporter = reporter;
            GnomeFileCache = gnomeFileCache;
        }

        public void ReadDefaultGnomeFile()
        {
            string defaultFile = GnomeFileCache.GetDefault();
            if (defaultFile != null) ReadGnomeFile(defaultFile);
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
            catch(DirectoryNotFoundException ex)
            {
                Reporter.AddMessage(new Message("I couldn't find the directory:"));
                Reporter.AddMessage(new Message(pathAndFile));
            }
            catch(FileNotFoundException ex)
            {
                Reporter.AddMessage(new Message("I couldn't find the file:"));
                Reporter.AddMessage(new Message(pathAndFile));
            }
            catch(Exception ex)
            {
                Reporter.AddMessage(new Message("I don't know why that didn't work:"));
                Reporter.AddMessage(new Message(ex.Message));
            }
        }
    }
}
