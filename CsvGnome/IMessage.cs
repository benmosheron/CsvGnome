using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// A message that can be reported back to the user by an IReporter.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// The message.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Return a list containing only this message.
        /// </summary>
        List<IMessage> ToList();
    }
}
