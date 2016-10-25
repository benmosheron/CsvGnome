using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Reports info back to the user.
    /// </summary>
    public interface IReporter
    {
        /// <summary>
        /// Add a message to be displayed to the user.
        /// </summary>
        void AddMessage(IMessage message);

        /// <summary>
        /// Add a message to be displayed to the user.
        /// </summary>
        void AddMessage(string message);

        /// <summary>
        /// Add a message to be displayed to the user.
        /// </summary>
        void AddMessage(string message, ConsoleColor colour);

        /// <summary>
        /// Report to the user that an error has occurred.
        /// </summary>
        void AddError(string message);

        /// <summary>
        /// Get the message provider which will provide valid messages for this reporter to use.
        /// </summary>
        IMessageProvider MessageProvider { get; }
    }
}
