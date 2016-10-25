using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Provides messages to components.
    /// </summary>
    public interface IMessageProvider
    {
        IMessage New(string message);
        IMessage New(string message, ConsoleColor colour);
        IMessage NewSpecial(string message);
        List<IMessage> EmptyList();
    }
}
