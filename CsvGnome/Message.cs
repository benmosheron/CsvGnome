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
    public class Message
    {
        public string Text { get; }

        public ConsoleColor Colour { get; }

        public Message(string s)
        :this(s, ConsoleColor.White)
        {  }

        public Message(string s, ConsoleColor c)
        {
            Text = s;
            Colour = c;
        }
    }
}
