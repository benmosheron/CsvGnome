using CsvGnome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeStandAlone
{
    /// <summary>
    /// Message to write to console once. Incorporates a single colour.
    /// </summary>
    public class Message : IMessage
    {
        public const ConsoleColor DefaultColour = ConsoleColor.Gray;
        public const ConsoleColor SpecialColour = ConsoleColor.Cyan;

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

        public static Message NewSpecial(string s)
        {
            return new Message(s, SpecialColour);
        }

        /// <summary>
        /// Return a List of Message with this as the only element.
        /// </summary>
        public List<IMessage> ToList()
        {
            return new List<IMessage>() { this };
        }

        public bool EqualsMessage(Message x)
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
