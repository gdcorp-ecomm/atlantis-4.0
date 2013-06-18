using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "testMode" element
    public class TestModeElement : ConfigurationElement
    {
        // Define the "enabled" attribute
        [ConfigurationProperty("enabled", DefaultValue = CaptchaDefaults.TestModeEnabled, IsRequired = false)]
        public Boolean Enabled
        {
            get
            {
                return (Boolean)this["enabled"];
            }
        }
    }
}
