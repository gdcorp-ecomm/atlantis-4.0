using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "captchaSessionTroubleshooting" element
    public class CaptchaSessionTroubleshootingElement : ConfigurationElement
    {
        // Define the "enabled" attribute
        [ConfigurationProperty("enabled", DefaultValue = CaptchaDefaults.CaptchaSessionTroubleshootingEnabled, IsRequired = false)]
        public Boolean Enabled
        {
            get
            {
                return (Boolean)this["enabled"];
            }
        }
    }
}
