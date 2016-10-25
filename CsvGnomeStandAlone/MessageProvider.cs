using CsvGnome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeStandAlone
{
    public class MessageProvider : IMessageProvider
    {
        public List<IMessage> EmptyList()
        {
            return new List<IMessage>();
        }

        public IMessage New(string message)
        {
            return new Message(message);
        }

        public IMessage New(string message, ConsoleColor colour)
        {
            return new Message(message, colour);
        }

        public IMessage NewSpecial(string message)
        {
            return Message.NewSpecial(message);
        }
    }
}
