using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Actions to take after interpreting user input.
    /// </summary>
    public enum GnomeAction
    {
        Exit,
        Continue,
        Run,
        Write,
        Help,
        HelpSpecial,
        Save
    }

    public class GnomeActionInfo
    {
        public readonly GnomeAction Action;

        public bool ContinueExecution =>
            Action == GnomeAction.Continue
                || Action == GnomeAction.Help
                || Action == GnomeAction.HelpSpecial;

        public readonly string Text = String.Empty;

        public GnomeActionInfo(GnomeAction action)
        {
            Action = action;
        }

        public GnomeActionInfo(GnomeAction action, string text)
        {
            Action = action;
            Text = text;
        }

        public static bool operator ==(GnomeActionInfo a, GnomeActionInfo b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Action == b.Action && a.Text == b.Text;
        }

        public static bool operator !=(GnomeActionInfo a, GnomeActionInfo b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to GnomeActionInfo return false.
            GnomeActionInfo p = obj as GnomeActionInfo;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return Action == p.Action && Text == p.Text;
        }

        public override int GetHashCode()
        {
            return (int)Action ^ Text.GetHashCode();
        }
    }
}
