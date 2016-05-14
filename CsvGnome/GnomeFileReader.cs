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
        Reporter Reporter;
        GnomeFileCache GnomeFileCache;

        public GnomeFileReader(Reporter reporter, GnomeFileCache gnomeFileCache)
        {
            Reporter = reporter;
            GnomeFileCache = gnomeFileCache;
        }

        public List<string> ReadDefaultGnomeFile()
        {
            List<string> parsedFile = new List<string>();
            string defaultFile = GnomeFileCache.DefaultGnomeFileName;
            if (defaultFile != null) parsedFile = ReadGnomeFile(defaultFile);
            return parsedFile;
        }

        public List<string> ReadGnomeFile(string name)
        {
            string pathAndFile = GnomeFileCache.GetGnomeFilePath(name.Trim());
            List<string> parsedFile = new List<string>();
            try {
                parsedFile = File.ReadAllLines(pathAndFile).ToList();
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

            return parsedFile;
        }
    }
}
