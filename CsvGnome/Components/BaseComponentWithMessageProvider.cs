using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components
{
    /// <summary>
    /// Base functionality for components - provides access to a message provider.
    /// </summary>
    public class BaseComponentWithMessageProvider
    {
        // Pulbic to allow combinatorial components to "inherit" from their raw components.
        public IMessageProvider MessageProvider;

        protected BaseComponentWithMessageProvider()
        {
            // Leave message provider null
        }

        /// <summary>
        /// Overload to use when the component's message will be output to the user.
        /// </summary>
        protected BaseComponentWithMessageProvider(IMessageProvider messageProvider)
        {
            MessageProvider = messageProvider;
        }
    }
}
