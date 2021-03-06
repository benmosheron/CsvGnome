﻿using CsvGnome;
using CsvGnome.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeStandAlone
{
    /// <summary>
    /// Reports data to the console.
    /// </summary>
    public class Reporter : IReporter
    {
        private List<Message> messages = new List<Message>();

        /// <summary>
        /// Report data on current fields, plus any messages, to the console.
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="N"></param>
        /// <param name="pathAndFile"></param>
        public void Report(List<IField> fields, long N, string pathAndFile)
        {
            Console.Clear();

            WriteMessages();

            Console.WriteLine($"Writing {N} rows to {pathAndFile}");
            Console.WriteLine(String.Empty);

            if (!fields.Any()) return;

            var names = fields.Select(f => f.Name);

            int maxLength = names.Max(f => f.Length);
            var namesForDisplay = names.Select(n => RightPad(n, maxLength)).ToList();

            for(int i = 0; i < fields.Count; i++)
            {
                Console.Write($"{namesForDisplay[i]}: ");
                foreach (Message m in fields[i].Summary) Write(m);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Register a message to be displayed.
        /// </summary>
        /// <param name="m"></param>
        public void AddMessage(Message m) => messages.Add(m);
        public void AddMessage(List<Message> m) => messages.AddRange(m);

        public void Help()
        {
            Console.Clear();

            Console.WriteLine("Enter a number to set the number of rows to write (doesn't include column row).");
            Console.WriteLine("run                     |writes the csv and exits.");
            Console.WriteLine("write                   |writes the csv and continues.");
            Console.WriteLine("exit                    |quits.");
            Console.WriteLine("clear                   |clears all fields.");
            Console.WriteLine("full on                 |displays arrays of values in full in the console.");
            Console.WriteLine("full off                |only display the number of items in arrays in the console.");
            Console.WriteLine("help                    |displays help.");
            Console.WriteLine("help special            |displays information on special values.");
            Console.WriteLine("gnomefiles              |displays information on saved gnomefiles.");
            Console.WriteLine("save                    |saves the currently set up fields to the currently loaded gnomefile.");
            Console.WriteLine("save file1              |saves the currently set up fields to gnomefile: \"file1\" in the gnomefile directory.");
            Console.WriteLine("load file1              |loads gnomefile \"file1\" from the gnomefile directory, overwriting any unsaved fields.");
            Console.WriteLine("delete field1           |deletes field \"field1\".");
            Console.WriteLine(@"output C:\path\file.csv |changes location to write the output CSV file to.");
            Console.WriteLine("");
            Console.WriteLine("To add a new field, use the following syntax:");
            Console.WriteLine("fieldName:fieldValue");
            Console.WriteLine("");
            Console.WriteLine("For example, a field called \"Name\" with all values \"Mogget\":");
            Console.WriteLine("Name:Mogget");
            Console.WriteLine("");
            Console.WriteLine("Special values can be included in square brackets, e.g.:");
            Console.WriteLine("Name:Mogget[date]_[++]");
            Console.WriteLine("");
            Console.WriteLine("Enter \"help special\" for a list of special values.");
            Console.WriteLine("Press [Enter] to return.");
        }

        public void HelpSpecial()
        {
            Console.Clear();

            Console.WriteLine("[++]");
            Console.WriteLine("  A simple counter: 0,1,2,...");
            Console.WriteLine("");
            Console.WriteLine("[10++]");
            Console.WriteLine("  A simple counter with starting point: 10,11,12,...");
            Console.WriteLine("");
            Console.WriteLine("[1++10]");
            Console.WriteLine("  A simple counter with starting point and increment:");
            Console.WriteLine("  1,11,21,...");
            Console.WriteLine("");
            Console.WriteLine("[1++2 every 3]");
            Console.WriteLine("  Only increments every 3 rows:");
            Console.WriteLine("  1,1,1,3,3,3,5,5,5,...");
            Console.WriteLine("  Can be given and ID to be used combinatorially.");
            Console.WriteLine("");
            Console.WriteLine("[date]");
            Console.WriteLine("  Includes the date and time (at the point of csv creation)");
            Console.WriteLine("");
            Console.WriteLine("[spread]{a,b,c}");
            Console.WriteLine("  Spreads the values between curly braces through the file: a,a,a,b,b,b,c,c");
            Console.WriteLine("");
            Console.WriteLine("[cycle]{a,b,c}");
            Console.WriteLine("  Cycles the values between curly braces through the file: a,b,c,a,b,c,a,b");
            Console.WriteLine("  Can be given and ID to be used combinatorially.");
            Console.WriteLine("");
            Console.WriteLine("[a=>z]");
            Console.WriteLine("  Cycles the characters from a to z.");
            Console.WriteLine("  Use a mixture of upper and lower case to cycle both cases. E.g. [b=>C]: b,c,B,C");
            Console.WriteLine("  Can be given and ID to be used combinatorially.");
            Console.WriteLine("");
            Console.WriteLine("[1=>3]");
            Console.WriteLine("  Repeats the numbers from 1 to 3 inclusive: 1,2,3,1,2,3...");
            Console.WriteLine("[1=>5, 2]");
            Console.WriteLine("  Repeats the numbers from 1 to 3 inclusive, in steps of 2: 1,3,5,1,3,5...");
            Console.WriteLine("");
            Console.WriteLine("Give two or more repeating values the same id with \"#\" to combinatorially cycle through values.");
            Console.WriteLine("  E.g. the command:");
            Console.WriteLine("positionField:([1=>3 #position]_[0=>1 #position])");
            Console.WriteLine("  will create the following csv:");
            Console.WriteLine("positionField");
            Console.WriteLine("(1_0)");
            Console.WriteLine("(1_1)");
            Console.WriteLine("(2_0)");
            Console.WriteLine("(2_1)");
            Console.WriteLine("(3_0)");
            Console.WriteLine("(3_1)");
            Console.WriteLine("");
            Console.WriteLine("Combined values can be in the same, or different fields.");
            Console.WriteLine("Press [Enter] to return.");
        }

        public void ShowGnomeFiles()
        {
            Console.Clear();
            Console.WriteLine($"The GnomeFile directory is: {Program.GetGnomeFileCache.GnomeFileDir}");
            Console.WriteLine("");
            for (int i = 0; i < Program.GetGnomeFileCache.Files.Length; i++)
            {
                string name = Program.GetGnomeFileCache.Files[i];
                WriteLine(new Message(name, Message.SpecialColour));
                WriteLine(new Message(Program.GetGnomeFileCache.GetPath(name)));
                Console.WriteLine("");
            }
        }

        public string OverrideConsole(List<Message> tempMessages)
        {
            Console.Clear();
            foreach(Message m in tempMessages)
            {
                WriteLine(m);
            }
            return Console.ReadLine();
        }

        public void AddError(string message)
        {
            AddError(new Message(message).ToList());
        }

        private void AddError(List<Message> tempMessages)
        {
            tempMessages.Insert(0, new Message("Error:", ConsoleColor.Red));
            OverrideConsole(tempMessages);
        }

        private void Write(Message m)
        {
            Console.ForegroundColor = m.Colour;
            Console.Write(m.Text);
            Console.ForegroundColor = Message.DefaultColour;
        }

        private void WriteLine(Message m)
        {
            Console.ForegroundColor = m.Colour;
            Console.WriteLine(m.Text);
            Console.ForegroundColor = Message.DefaultColour;
        }

        private void WriteMessages()
        {
            for (int i = 0; i < messages.Count; i++)
            {
                WriteLine(messages[i]);
            }
            messages.Clear();
        }

        private string RightPad(string s, int endLength)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Append(' ', endLength - sb.Length);
            return sb.ToString();
        }
    }
}
