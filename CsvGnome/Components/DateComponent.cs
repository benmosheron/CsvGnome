using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    class DateComponent : IComponent
    {
        public string Summary => Program.DateComponentString;
        public string GetValue(int i)
        {
            return GetDateTime();
        }

        /// <summary>
        /// String representation of date/time at runtime.
        /// </summary>
        private string GetDateTime()
        {
            return DateTime.Now.ToString(Program.DateTimeFormat);
        }
    }
}
