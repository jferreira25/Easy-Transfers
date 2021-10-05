using Easy.Transfers.CrossCutting.Configuration.SettingsReader;
using System;
using System.IO;

namespace Easy.Transfers.CrossCutting.Configuration
{

    [Serializable]
    public static class AppFileConfiguration<TSetting> where TSetting : Settings
    {
        private static TSetting _settings;

        public static TSetting Settings => _settings ?? (_settings = LoadSettingsFile<TSetting>());

        public static TSetting GetSettingsFromDifferentFile(string filename)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));
            return _settings ?? (_settings = LoadSettingsFile<TSetting>(filename));
        }

        private static TSettingConfig LoadSettingsFile<TSettingConfig>()
        {
            return LoadSettingsFile<TSettingConfig>(AppFileConfiguration.ApplicationSettingsFilePath ?? $"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json");
        }

        private static TSettingConfig LoadSettingsFile<TSettingConfig>(string filename)
        {
            return FileSettingsReader.LoadSettings<TSettingConfig>(filename);
        }

       
        public static void ResetSettings()
        {
            _settings = null;
        }
    }

  
    public static class AppFileConfiguration
    {
       
        internal static string ApplicationSettingsFilePath { get; private set; }

        public static void AddDifferentApplicationSettingsFile(string filePath)
        {
            if (filePath != null && !File.Exists(filePath))
                throw new ArgumentException("Provided file path does not exists. Please check the file path and try again", nameof(filePath));

            ApplicationSettingsFilePath = filePath;
        }
    }
}