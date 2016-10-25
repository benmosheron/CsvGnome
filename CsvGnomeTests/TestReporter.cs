using CsvGnome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeTests
{
    public class TestReporter : IReporter
    {
        public IMessageProvider MessageProvider
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void AddError(string message)
        {
            throw new NotImplementedException();
        }

        public void AddMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void AddMessage(IMessage message)
        {
            throw new NotImplementedException();
        }

        public void AddMessage(string message, ConsoleColor colour)
        {
            throw new NotImplementedException();
        }
    }
}
