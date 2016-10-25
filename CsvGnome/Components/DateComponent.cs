using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components
{
    public class DateComponent : BaseComponentWithMessageProvider, IComponent
    {
        public const string NoFormatCommand = "[date]";
        
        #region IComponent
        public string Command
        {
            get
            {
                if (Format == String.Empty) return NoFormatCommand;
                else return $"[date \"{Format}\"]";
            }
        }
        public List<IMessage> Summary => MessageProvider.NewSpecial(Command).ToList();
        public string GetValue(long i)
        {
            return GetValue();
        }
        public bool EqualsComponent(IComponent x)
        {
            if (x == null) return false;
            var c = x as DateComponent;
            if (c == null) return false;
            if (GetValue(0) != c.GetValue(0)) return false;
            return true;
        }
        #endregion

        Date.IProvider DateProvider;
        string Format;

        public DateComponent(Date.IProvider dateProvider, string format = "", IMessageProvider messageProvider = null)
            : base(messageProvider)
        {
            DateProvider = dateProvider;
            Format = format;
        }

        string GetValue()
        {
            return DateProvider.Get().ToString(Format);
        }
    }
}
