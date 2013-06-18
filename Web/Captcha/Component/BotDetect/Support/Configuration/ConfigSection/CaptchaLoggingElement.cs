using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "captchaLogging" element
    public class CaptchaLoggingElement : ConfigurationElement
    {
        // Create a "errorLoggingEnabled" element.
        [ConfigurationProperty("errorLoggingEnabled", DefaultValue = CaptchaDefaults.ErrorLoggingEnabled, IsRequired = false)]
        public Boolean ErrorLoggingEnabled
        {
            get
            {
                return (Boolean)this["errorLoggingEnabled"];
            }
        }

        // Create a "traceEnabled" element.
        [ConfigurationProperty("traceEnabled", DefaultValue = CaptchaDefaults.TraceEnabled, IsRequired = false)]
        public Boolean TraceEnabled
        {
            get
            {
                return (Boolean)this["traceEnabled"];
            }
        }

        // Create a "eventFilter" element.
        [ConfigurationProperty("eventFilter", DefaultValue = CaptchaDefaults.EventFilter, IsRequired = false)]
        public String EventFilter
        {
            get
            {
                return (String)this["eventFilter"];
            }
        }

        // Create a "loggingProvider" element.
        [ConfigurationProperty("loggingProvider", DefaultValue = CaptchaDefaults.LoggingProvider, IsRequired = false)]
        public String LoggingProvider
        {
            get
            {
                return (String)this["loggingProvider"];
            }
        }
    }
}
