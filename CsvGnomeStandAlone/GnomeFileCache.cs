﻿using CsvGnome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeStandAlone
{
    /// <summary>
    /// A cache for gnomefiles in the gnomefile directory.
    /// </summary>
    public class GnomeFileCache
    {
        public const string DefaultGnomeFileName = "default";
        public const string GnomeFileExt = ".gnome";
        public string GnomeFileDir;
        Dictionary<string, string> Cache;
        Reporter Reporter;

        public GnomeFileCache(Reporter reporter)
        {
            Cache = new Dictionary<string, string>();
            Reporter = reporter;

            string dir = String.Empty;
            try
            {
                dir = Directory.GetCurrentDirectory();
            }
            catch (UnauthorizedAccessException ex)
            {
                Reporter.AddMessage(new Message($"I couldn't access the default GnomeFile directory. [{dir}] {ex.Message}"));
            }

            GnomeFileDir = Path.Combine(dir, "GnomeFiles");
            Update();
        }

        public void Update()
        {
            if (Directory.Exists(GnomeFileDir))
            {
                // cache up
                Dictionary<string,string> gnomeFiles = Directory.GetFiles(GnomeFileDir)
                    .Where(g => Path.GetExtension(g) == GnomeFileExt)
                    .ToDictionary<string, string>(g => Path.GetFileNameWithoutExtension(g));

                foreach (var k in gnomeFiles.Keys)
                {
                    // Update existing
                    if (Cache.ContainsKey(k)) Cache[k] = gnomeFiles[k];
                    // Add new file
                    else Cache.Add(k, gnomeFiles[k]);
                }
            }
            else
            {
                Reporter.AddMessage(new Message("I couldn't find the default GnomeFile directory at:"));
                Reporter.AddMessage(new Message(GnomeFileDir));
            }
        }

        public string[] Files => Cache.Keys.ToArray();

        public string GetDefault()
        {
            string s = null;
            if (Cache.ContainsKey(DefaultGnomeFileName))
            {
                s = Cache[DefaultGnomeFileName];
            }
            else
            {
                // try updating
                Update();
                if (Cache.ContainsKey(DefaultGnomeFileName))
                {
                    s = Cache[DefaultGnomeFileName];
                }
                else
                {
                    Reporter.AddMessage(new Message("I couldn't find the default GnomeFile at:"));
                    Reporter.AddMessage(new Message(Path.Combine(GnomeFileDir, $"{DefaultGnomeFileName}{GnomeFileExt}")));
                }
            }
            return s;
        }

        public string GetPath(string name) => Cache[name];

        public string GetGnomeFilePath(string name)
        {
            string path = Path.Combine(GnomeFileDir, name);
            path = Path.ChangeExtension(path, GnomeFileExt);
            return path;
        }

        public void AddToCache(string name)
        {
            string path = GetGnomeFilePath(name);
            if (Cache.ContainsKey(name)) Cache[name] = path;
            else Cache.Add(name, path);
        }

        public bool Exists(string name)
        {
            return Cache.ContainsKey(name);
        }

        public bool ExistsAfterUpdate(string name)
        {
            Update();
            return Cache.ContainsKey(name);
        }
    }
}
