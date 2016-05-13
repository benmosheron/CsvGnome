using System;
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

            Console.WriteLine($"Writing {N} rows to {pathAndFile}");

            if (!fields.Any()) return;

            var names = fields.Select(f => f.Name);
            var values = fields.Select(f => f.Summary).ToList();

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
