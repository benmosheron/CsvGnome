using CsvGnome;
using CsvGnome.GnomeFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeStandAlone
{
    /// <summary>
    /// Reads gnome files, which save the configuration of a set of fields for re-use.
    /// </summary>
    /// <remarks>
    /// Only parses files to lists of strings, doesn't interpret them.
    /// </remarks>
    public class GnomeFileReader : IReader
    {
        Reporter Reporter;
        GnomeFileCache GnomeFileCache;

        private string lastLoadedFileName = null;

        public string LastLoadedFileName => lastLoadedFileName;

        public GnomeFileReader(Reporter reporter, GnomeFileCache gnomeFileCache)
        {
            Reporter = reporter;
            GnomeFileCache = gnomeFileCache;
        }

        public List<string> ReadDefaultGnomeFile()
        {
            List<string> parsedFile = new List<string>();
            string defaultFile = GnomeFileCache.DefaultGnomeFileName;
            if (defaultFile != null) parsedFile = ReadGnomeFileFromCache(defaultFile);
            return parsedFile;
        }

        public List<string> ReadGnomeFileFromCache(string name)
        {
            string pathAndFile = GnomeFileCache.GetGnomeFilePath(name.Trim());
            List<string> parsedFile = new List<string>();
            try {
                parsedFile = File.ReadAllLines(pathAndFile).ToList();
            }
            catch(DirectoryNotFoundException)
            {
                Reporter.AddMessage(new Message("I couldn't find the directory:"));
                Reporter.AddMessage(new Message(pathAndFile));
            }
            catch(FileNotFoundException)
            {
                Reporter.AddMessage(new Message("I couldn't find the file:"));
                Reporter.AddMessage(new Message(pathAndFile));
            }
            catch(Exception ex)
            {
                Reporter.AddMessage(new Message("I don't know why that didn't work:"));
                Reporter.AddMessage(new Message(ex.Message));
            }

            lastLoadedFileName = name;

            Program.SetConsoleTitle();

            return parsedFile;
        }

        public List<string> ReadGnomeFile(string path)
        {
            throw new NotImplementedException();
        }

        List<string> IReader.ReadGnomeFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
