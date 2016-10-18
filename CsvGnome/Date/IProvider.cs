using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Date
{
    /// <summary>
    /// Provides date information to date components.
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// Get the stored DateTime.
        /// </summary>
        DateTime Get();

        /// <summary>
        /// Set the stored DateTime to now.
        /// </summary>
        void UpdateNow();
    }
}
