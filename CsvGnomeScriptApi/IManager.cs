using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeScriptApi
{
    /// <summary>
    /// Reads script files and stores functions read from them.
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// Read a script file, storing valid functions.
        /// </summary>
        void ReadFile(string path);

        /// <summary>
        /// Retrieve a function that has previously been read from a script file.
        /// </summary>
        /// <param name="language">E.g. "lua"</param>
        /// <param name="functionName">E.g. "square"</param>
        /// <returns></returns>
        Func<long, object[]> GetValueFunction(string language, string functionName);
    }
}
