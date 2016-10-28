using System;

namespace CsvGnome.Date
{
    /// <summary>
    /// Provides the DateTime of an instance's creation, or when UpdateNow() was last called.
    /// </summary>
    public class Provider : IProvider
    {
        private DateTime date = DateTime.Now;

        public DateTime Get()
        {
            return date;
        }

        public void UpdateNow()
        {
            date = DateTime.Now;
        }
    }
}
