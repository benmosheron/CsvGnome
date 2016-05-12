﻿using System;
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
        Dictionary<string, string> gnomeFileCache;

        public GnomeFileReader(Interpreter interpreter)
        {
            Interpreter = interpreter;
        }

        public void ReadDefaultGnomeFile()
        {
            string dir = Directory.GetCurrentDirectory();
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
                throw new NotImplementedException("directory of default file not found");
            }
            catch(FileNotFoundException ex)
            {
                throw new NotImplementedException("default file not found");
            }
        }
    }
}
