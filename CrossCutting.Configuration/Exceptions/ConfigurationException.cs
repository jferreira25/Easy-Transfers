using System;

namespace Easy.Transfers.CrossCutting.Configuration.Exceptions
{
    public class ConfigurationException : Exception
    {
        private const string MessageText = "A configuration exception has occurred.";

        public ConfigurationException() : base(MessageText)
        {
        }

        public ConfigurationException(Exception innerException) : base(MessageText, innerException)
        {
        }

        public ConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}