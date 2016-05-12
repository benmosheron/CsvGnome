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
        Reporter Reporter;
        Dictionary<string, string> gnomeFileCache;

        public GnomeFileReader(Interpreter interpreter, Reporter reporter)
        {
            Interpreter = interpreter;
            Reporter = reporter;
        }

        public void ReadDefaultGnomeFile()
        {
            string dir = String.Empty;
            try
            {
                dir = Directory.GetCurrentDirectory();
            }
            catch(UnauthorizedAccessException ex)
            {
                Reporter.AddMessage(new Message("I couldn't access the default GnomeFile directory."));
            }

            string gnomeDir = Path.Combine(dir, "GnomeFiles");
            if (Directory.Exists(gnomeDir))
            {
                // cache up
                gnomeFileCache = Directory.GetFiles(gnomeDir)
                    .Where(g => Path.GetExtension(g) == Program.GnomeFileExt)
                    .ToDictionary<string, string>(g => Path.GetFileNameWithoutExtension(g));

                if (gnomeFileCache.ContainsKey("default"))
                {
                    ReadGnomeFile(gnomeFileCache["default"]);
                }
                else
                {
                    Reporter.AddMessage(new Message("I couldn't find the default GnomeFile at:"));
                    Reporter.AddMessage(new Message(Path.Combine(gnomeDir, $"{Program.DefaultGnomeFileName}{Program.GnomeFileExt}")));
                }
            }
            else
            {
                Reporter.AddMessage(new Message("I couldn't find the default GnomeFile directory at:"));
                Reporter.AddMessage(new Message(gnomeDir));
            }
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
