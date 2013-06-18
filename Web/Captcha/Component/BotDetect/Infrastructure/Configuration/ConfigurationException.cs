using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect.Configuration
{
    [Serializable]
    public class ConfigurationException : BaseException
    {
        const string message = "An error occured in the BotDetect.Configuration wrapper";

        public ConfigurationException() 
            : base(message)
        {
        }

        public ConfigurationException(string auxMessage)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

        public ConfigurationException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public ConfigurationException(string auxMessage, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public ConfigurationException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected ConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
