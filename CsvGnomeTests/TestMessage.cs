using CsvGnome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeTests
{
    public class TestMessage : IMessage
    {
        public bool IsSpecial { get; } = false;

        private TestMessage(string message, bool special)
            :this(message)
        {
            IsSpecial = special;
        }

        public TestMessage(string message)
        {
            Text = message;
        }

        public static TestMessage NewSpecial(string message) => new TestMessage(message, true);

        public bool EqualsMessage(TestMessage m)
        {
            if (m == null) return false;
            if (m.Text != Text) return false;
            if (m.IsSpecial != IsSpecial) return false;
            return true;
        }

        public string Text
        {
            get; set;
        }

        public List<IMessage> ToList()
        {
            throw new NotImplementedException();
        }
    }
}
