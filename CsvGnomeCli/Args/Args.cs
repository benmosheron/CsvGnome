using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli.Args
{
    public static class Args
    {
        public static ArgBase File = new ArgBase("--file", "-f");
        public static ArgBase Output = new ArgBase("--output", "-o");
        //public static CliArg Preview = new CliArg("--preview", "-p");
        //public static CliArg CreateDirectories = new CliArg("--file", "-f");

        public static List<ArgBase> GetAll()
        {
            return new List<ArgBase>()
            {
                File,
                Output
            };
        }
    }
}
