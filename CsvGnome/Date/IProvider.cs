using System;

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
