using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Configuration
{
    public class Provider : IProvider
    {
        private const string PadOutputKey = "PadOutput";
        private const string ReportArrayContentsKey = "ReportArrayContents";
        private bool? padOutput;
        private bool? reportArrayContents;
        private readonly Reporter Reporter;

        /// <summary>
        /// Create a ConfigurationProvider that will swallow any exceptions and report errors instead of crashing.
        /// </summary>
        /// <param name="reporter"></param>
        public Provider(Reporter reporter)
        {
            Reporter = reporter;
        }

        #region IProvider

        public bool PadOutput
        {
            get
            {
                if (!padOutput.HasValue)
                {
                    padOutput = GetConfig(PadOutputKey, Defaults.PadOutput);
                }
                return padOutput.Value;
            }
        }

        /// <summary>
        /// If true, array contents will be written to the console. If false, only the number of items will be displayed.
        /// </summary>
        public bool ReportArrayContents
        {
            get
            {
                if (!reportArrayContents.HasValue)
                {
                    reportArrayContents = GetConfig(ReportArrayContentsKey, Defaults.ReportArrayContents);
                }
                return reportArrayContents.Value;
            }
        }

        public void SetPadOutput(bool value)
        {
            padOutput = value;
            SetConfig(PadOutputKey, value.ToString());
        }

        public void SetReportArrayContents(bool value)
        {
            reportArrayContents = value;
            SetConfig(ReportArrayContentsKey, value.ToString());
        }

        #endregion

        private void SetConfig(string key, string value)
        {
            try
            {
                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove(key);
                config.AppSettings.Settings.Add(key, value);
                config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            }
            catch(Exception ex)
            {
                Reporter.AddMessage(new Message(ex.Message, ConsoleColor.Red));
            }
        }

        private string GetConfig(string key, string def)
        {
            try
            {
                string config = System.Configuration.ConfigurationManager.AppSettings[key];
                if (String.IsNullOrEmpty(config)) return def;
                else return config;
            }
            catch (Exception ex)
            {
                Reporter.AddMessage(new Message(ex.Message, ConsoleColor.Red));
                return def;
            }
        }

        private bool GetConfig(string key, bool def)
        {
            string config = GetConfig(key, def.ToString());
            bool temp;
            if (Boolean.TryParse(config, out temp))
            {
                return temp;
            }
            else
            {
                return def;
            }
        }
    }
}
