﻿using System;
using System.Collections.Generic;

namespace CsvGnome
{
    /// <summary>
    /// Message to write to console once. Incorporates a single colour.
    /// </summary>
    public class Message
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
        public List<Message> ToList()
        {
            return new List<Message>() { this };
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
