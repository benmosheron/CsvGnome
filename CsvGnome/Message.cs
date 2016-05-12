using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Message to write to console once.
    /// </summary>
    class Message
    {
        public string Text { get; }

        public Message(string s)
        {
            Text = s;
        }
    }
}
