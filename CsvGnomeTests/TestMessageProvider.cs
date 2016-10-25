using CsvGnome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeTests
{
    public class TestMessageProvider : IMessageProvider
    {
        public List<IMessage> EmptyList()
        {
            throw new NotImplementedException();
        }

        public IMessage New(string message)
        {
            throw new NotImplementedException();
        }

        public IMessage New(string message, ConsoleColor colour)
        {
            throw new NotImplementedException();
        }

        public IMessage NewSpecial(string message)
        {
            throw new NotImplementedException();
        }
    }
}
