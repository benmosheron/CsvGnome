﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Reports data to the console.
    /// </summary>
    public class Reporter
    {
        private List<Message> messages = new List<Message>();

        /// <summary>
        /// Report data on current fields, plus any messages, to the console.
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="N"></param>
        /// <param name="pathAndFile"></param>
        public void Report(List<IField> fields, int N, string pathAndFile)
        {
            Console.Clear();

            WriteMessages();

            Console.WriteLine($"\"run\" or \"write\" to write {N} rows to {pathAndFile}");

            if (!fields.Any()) return;

            var names = fields.Select(f => f.Name);
            var values = fields.Select(f => f.Command).ToList();

            int maxLength = names.Max(f => f.Length);
            var namesForDisplay = names.Select(n => RightPad(n, maxLength)).ToList();

            for(int i = 0; i < fields.Count; i++)
            {
                Console.WriteLine($"{namesForDisplay[i]}: {values[i]}");
            }
        }

        /// <summary>
        /// Register a message to be displayed.
        /// </summary>
        /// <param name="m"></param>
        public void AddMessage(Message m) => messages.Add(m);

        public void Help()
        {
            Console.Clear();

            Console.WriteLine("\"run\" writes the csv and exits.");
            Console.WriteLine("\"write\" writes the csv and continues.");
            Console.WriteLine("\"exit\" quits.");
            Console.WriteLine("\"help\" displays help.");
            Console.WriteLine("\"help special\" displays information on special values.");
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
            Console.WriteLine("[date]");
            Console.WriteLine("  Includes the date and time (at the point of csv creation)");
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
            Console.WriteLine("Combined values can be in the same, or different fields.");
        }

        private void WriteMessages()
        {
            for (int i = 0; i < messages.Count; i++)
            {
                Console.WriteLine(messages[i].Text);
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
