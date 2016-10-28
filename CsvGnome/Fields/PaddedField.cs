using System;
using System.Collections.Generic;

namespace CsvGnome.Fields
{
    #region Exceptions
    /// <summary>
    /// Thrown if GetValue(i) is called on a padded field before the max length has been calculated.
    /// </summary>
    public class PaddedLengthNotCalculatedException : Exception
    {
        public PaddedLengthNotCalculatedException()
            : this("GetValue(i) or GetPaddedName() was called by a PaddedField before CalculateMaxLength() was called.")
        { }

        private PaddedLengthNotCalculatedException(string message) : base(message) { }
    }

    /// <summary>
    /// Thrown if the length of any GetValue(i) result is larger than the calculated maximum.
    /// </summary>
    public class PaddedLengthExceededException : Exception
    {
        public long? RowNumber;
        public int ExpectedMaxLength;
        public int ActualLength;
        public string Value;

        private const string standardMessage =
@"A few things could have gone wrong, including:
   1) The field contained a [lua] component and has evaluated a function which returned a value with a higher length than it did in the dry run (with the same input parameters!).
      To fix either:
       1a) make sure all lua functions return a value with a deterministic length (when converted to a string) for any given row index.
       1b) turn off padding (command: pad off).
   2) CalculateMaxLength was not called, or was called for wrong N. 
      Congratulations! You may have discovered a bug! You can report it here: https://github.com/benmosheron/CsvGnome/issues/new
      To workaroun, turn off padding (command: pad off).";

        public PaddedLengthExceededException(int expectedMaxLength, string name)
            : this($"Name of field [{name}] is long than the expected maximum [{expectedMaxLength}]. {standardMessage}")
        {
            ExpectedMaxLength = expectedMaxLength;
            ActualLength = name.Length;
            Value = name;
        }

        public PaddedLengthExceededException(long i, int expectedMaxLength, int actualLength, string value)
            : this($"GetValue({i}) returned a value longer than the expected maximum [{expectedMaxLength}]. Value: [{value}]. Length: [{actualLength}]. {standardMessage}")
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
        public PaddedField(IField f)
        {
            InnerField = f;
        }

        /// <summary>
        /// Calls GetValue for each integer 1 -> N inclusive, stores the maximum length. O(N).
        /// </summary>
        public void CalculateMaxLength(long N)
        {
            int length = Name.Length;
            int tempLength;
            for (long i = 0; i < N; i++)
            {
                tempLength = InnerField.GetValue(i).Length;
                if (tempLength > length) length = tempLength;
            }

            MaxLength = length;
        }

        public string GetPaddedName()
        {
            if (!MaxLength.HasValue) throw new PaddedLengthNotCalculatedException();

            // Can't currently be hit (as we can't mess with the name), but better safe than sorry.
            if(Name.Length > MaxLength.Value) throw new PaddedLengthExceededException(MaxLength.Value, Name);

            string pad = String.Empty;

            if (Name.Length < MaxLength.Value)
            {
                pad = new string(' ', MaxLength.Value - Name.Length);
            }

            return Name + pad;
        }

        #region IField

        public string Command => InnerField.Command;
        public List<Message> Summary => InnerField.Summary;
        public string Name => InnerField.Name;
        public string GetValue(long i)
        {
            if (!MaxLength.HasValue) throw new PaddedLengthNotCalculatedException();

            // Pad the inner field's value with spaces up to the max length.
            string inner = InnerField.GetValue(i);

            if(inner.Length > MaxLength.Value) throw new PaddedLengthExceededException(i, MaxLength.Value, inner.Length, inner);

            string pad = String.Empty;

            if(inner.Length < MaxLength.Value)
            {
                pad = new string(' ', MaxLength.Value - inner.Length);
            }

            return inner + pad;
        }

        #endregion
    }
}
