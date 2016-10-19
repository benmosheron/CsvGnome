using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Fields
{
    #region Exceptions
    /// <summary>
    /// Thrown if GetValue(i) is called on a padded field before the max length has been calculated.
    /// </summary>
    public class PaddedLengthNotCalculatedException : Exception
    {
        public PaddedLengthNotCalculatedException()
            : this("GetValue(i) was called by a PaddedField before CalculateMaxLength() was called.")
        { }

        private PaddedLengthNotCalculatedException(string message) : base(message) { }
    }

    /// <summary>
    /// Thrown if the length of any GetValue(i) result is larger than the calculated maximum.
    /// </summary>
    public class PaddedLengthExceededException : Exception
    {
        public int RowNumber;
        public int ExpectedMaxLength;
        public int ActualLength;
        public string Value;
        public PaddedLengthExceededException(int i, int expectedMaxLength, int actualLength, string value)
            : this($"GetValue({i}) returned a value longer than the expected maximum [{expectedMaxLength}]. Value: [{value}]. Length: [{actualLength}].")
        {
            RowNumber = i;
            ExpectedMaxLength = expectedMaxLength;
            ActualLength = actualLength;
            Value = value;
        }

        private PaddedLengthExceededException(string message) : base(message) { }
    }
    #endregion Exceptions

    /// <summary>
    /// <para>Field which can calculate the maximum length of any GetValue() result, and therefore pad each output to a constant length with spaces.</para>
    /// <para>If N changes, you must call CalculateMaxLength again.</para>
    /// </summary>
    public class PaddedField : IPaddedField
    {
        private IField InnerField;

        private int? MaxLength;

        /// <summary>
        /// Create a new PaddedField, which will pad the input field with spaces.
        /// </summary>
        public PaddedField(ComponentField f)
        {
            InnerField = f;
        }

        /// <summary>
        /// Calls GetValue for each integer 1 -> N inclusive, stores the maximum length. O(N).
        /// </summary>
        public void CalculateMaxLength(int N)
        {
            int length = 0;
            int tempLength;
            for (int i = 0; i < N; i++)
            {
                tempLength = InnerField.GetValue(i).Length;
                if (tempLength > length) length = tempLength;
            }
            MaxLength = length;
        }

        #region IField

        public string Command => InnerField.Command;
        public string Name => InnerField.Name;
        public List<Message> Summary => InnerField.Summary;

        public string GetValue(int i)
        {
            if (!MaxLength.HasValue) throw new PaddedLengthNotCalculatedException();

            // Pad the inner field's value with spaces up to the max length.
            string inner = InnerField.GetValue(i);

            string pad = String.Empty;

            if(inner.Length <= MaxLength)
            {
                pad = new string(' ', MaxLength.Value - inner.Length);
            }
            else
            {
                throw new PaddedLengthExceededException(i, MaxLength.Value, inner.Length, inner);
            }

            return inner + pad;
        }

        #endregion
    }
}
