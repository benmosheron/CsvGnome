using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeScriptApi
{
    /// <summary>
    /// Provides access to a dictionary of value generating functions for a single scripting language.
    /// </summary>
    public interface IScriptFunctions
    {
        /// <summary>
        /// Maps function names to Value Generating Functions.
        /// </summary>
        Dictionary<string, Func<IScriptArgs, object[]>> ValueFunctions { get; }

        /// <summary>
        /// True if this contains a function with the given name.
        /// </summary>
        bool ContainsFunction(string name);

        /// <summary>
        /// Add every function from <paramref name="functionsToAdd"/> to this instance.
        /// </summary>
        void Combine(IScriptFunctions functionsToAdd);
    }
}
