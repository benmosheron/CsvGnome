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
        public void AddError(string message)
        {
            throw new NotImplementedException();
        }

        public void AddMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public void AddMessage(List<Message> messages)
        {
            throw new NotImplementedException();
        }
    }
}
