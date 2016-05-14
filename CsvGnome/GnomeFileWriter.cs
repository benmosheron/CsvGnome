using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class GnomeFileWriter
    {
        Reporter Reporter;
        FieldBrain FieldBrain;
        GnomeFileCache GnomeFileCache;

        public GnomeFileWriter(Reporter reporter, FieldBrain fieldBrain, GnomeFileCache gnomeFileCache)
        {
            Reporter = reporter;
            FieldBrain = fieldBrain;
            GnomeFileCache = gnomeFileCache;
        }

        public void Save(string fileName)
        {
            if (GnomeFileCache.ExistsAfterUpdate(fileName))
            {
                string ok = Reporter.OverrideConsole(AskOverwriteMessage(GnomeFileCache.GetPath(fileName)));
                if (!new string[] { "y", "yes" }.Contains(ok.Trim().ToLower())) return;
            }

            string path = GnomeFileCache.AddNewFile(fileName);

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(Program.N);

                for (int i = 0; i < FieldBrain.Fields.Count; i++)
                {
                    sw.Write($"{FieldBrain.Fields[i].Name}:{FieldBrain.Fields[i].Command}");
                }
            }
        }

        private List<Message> AskOverwriteMessage(string file)
        {
            return new List<Message>
            {
                new Message("The file already exists:"),
                new Message(file),
                new Message("OK to overwrite? (y/n)")
            };
        }

    }
}
