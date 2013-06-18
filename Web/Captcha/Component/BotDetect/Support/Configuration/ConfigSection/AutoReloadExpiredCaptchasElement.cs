using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "autoReloadExpiredCaptchas" element
    public class AutoReloadExpiredCaptchasElement : ConfigurationElement
    {
        // Create an "enabled" element.
        [ConfigurationProperty("enabled", DefaultValue = CaptchaDefaults.AutoReloadExpiredCaptchas, IsRequired = false)]
        public Boolean Enabled
        {
            get
            {
                return (Boolean)this["enabled"];
            }
        }

        // Define the "timeout" attribute
        [ConfigurationProperty("timeout", DefaultValue = 7200, IsRequired = false)]
        public int Timeout
        {
            get
            {
                return (int)this["timeout"];
            }
        }
    }
}
