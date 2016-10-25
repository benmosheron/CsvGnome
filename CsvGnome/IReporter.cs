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
        void AddMessage(List<Message> messages);

        /// <summary>
        /// Add a message to be displayed to the user.
        /// </summary>
        void AddMessage(Message message);

        /// <summary>
        /// Report to the user that an error has occurred.
        /// </summary>
        void AddError(string message);

    }
}
