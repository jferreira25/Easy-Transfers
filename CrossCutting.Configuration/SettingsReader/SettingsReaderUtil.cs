using System;
using System.Text.RegularExpressions;

namespace Easy.Transfers.CrossCutting.Configuration.SettingsReader
{
    internal static class SettingsReaderUtil
    {
        public static string InjectEnvironmentVariables(string applicationSettingsInput)
        {
            var returnValue = applicationSettingsInput;
            var environmentVariables = Environment.GetEnvironmentVariables();

            foreach (System.Collections.DictionaryEntry environmentVariable in environmentVariables)
            {
                var value = environmentVariable.Value?.ToString() ?? string.Empty;
                returnValue = returnValue.Replace($"${{{environmentVariable.Key}}}", Regex.Replace(value, @"\\", @"\\"));
            }

            returnValue = Regex.Replace(returnValue, @"\$\{(.*)\}", string.Empty);
            return returnValue;
        }
    }
}