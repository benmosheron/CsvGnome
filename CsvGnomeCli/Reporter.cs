using CsvGnome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli
{
    public class Reporter : IReporter
    {
        public void AddError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void AddMessage(Message message)
        {
            Console.ForegroundColor = message.Colour;
            Console.WriteLine(message.Text);
            Console.ResetColor();
        }

        public void AddMessage(List<Message> messages)
        {
            messages.ForEach(AddMessage);
        }
    }
}
