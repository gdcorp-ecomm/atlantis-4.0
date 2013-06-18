using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "captchaReloading" element
    public class CaptchaReloadingElement : ConfigurationElement
    {
        // Define the "enabled" attribute
        [ConfigurationProperty("enabled", DefaultValue = CaptchaDefaults.ReloadingEnabled, IsRequired = false)]
        public Boolean Enabled
        {
            get
            {
                return (Boolean)this["enabled"];
            }
        }

        // Define the "reloadIcon" sub-element
        [ConfigurationProperty("reloadIcon", IsRequired = false)]
        public ReloadIconElement ReloadIcon
        {
            get
            {
                return (ReloadIconElement)this["reloadIcon"];
            }
        }

        // Define the "autoReloadExpiredCaptchas" sub-element
        [ConfigurationProperty("autoReloadExpiredCaptchas", IsRequired = false)]
        public AutoReloadExpiredCaptchasElement AutoReloadExpiredCaptchas
        {
            get
            {
                return (AutoReloadExpiredCaptchasElement)this["autoReloadExpiredCaptchas"];
            }
        }
    }
}
