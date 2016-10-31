using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvGnome;

namespace CsvGnomeCliTest
{
    // Contains a few IReporter implementations for testing
    public class ThrowReporter : IReporter
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

    public class SingleMessageReporter : IReporter
    {
        public Message msg = null;

        public void AddError(string message)
        {
            throw new NotImplementedException();
        }

        public void AddMessage(Message message)
        {
            if (msg != null) throw new NotImplementedException("Single message already set");
            msg = message;
        }

        public void AddMessage(List<Message> messages)
        {
            if (messages.Count > 1) throw new NotImplementedException("Multiple messages supplied to SingleMessageReporter");
            AddMessage(messages.First());
        }
    }

    public class SingleErrorReporter : IReporter
    {
        public string msg = null;

        public void AddError(string message)
        {
            if (msg != null) throw new NotImplementedException("Single error message already set");
            msg = message;
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
