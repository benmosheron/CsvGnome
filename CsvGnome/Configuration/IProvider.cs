namespace CsvGnome.Configuration
{
    /// <summary>
    /// Provides access to configured app settings.
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// Whether or not output should be padded with spaces so that columns are aligned visually.
        /// </summary>
        bool PadOutput { get; }

        /// <summary>
        /// Whether or not array contents (for [spread] and [cycle] components) are written in full in the console window.
        /// </summary>
        bool ReportArrayContents { get; }

        void SetPadOutput(bool value);

        void SetReportArrayContents(bool value);
    }
}
