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
        :this(s, ConsoleColor.Gray)
        {  }

        public Message(string s, ConsoleColor c)
        {
            Text = s;
            Colour = c;
        }

        /// <summary>
        /// Return a List of Message with this as the only element.
        /// </summary>
        public List<Message> ToList()
        {
            return new List<Message>() { this };
        }

        public bool Equals(Message x)
        {
            if (x == null) return false;
            var c = x as Message;
            if (c == null) return false;
            if (Text != c.Text) return false;
            if (Colour != c.Colour) return false;
            return true;
        }
    }
}
