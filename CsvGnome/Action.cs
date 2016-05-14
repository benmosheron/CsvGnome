using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Actions to take after interpreting user input.
    /// </summary>
    public enum Action
    {
        Exit,
        Continue,
        Run,
        Write,
        Help,
        HelpSpecial,
        Save
    }
}
