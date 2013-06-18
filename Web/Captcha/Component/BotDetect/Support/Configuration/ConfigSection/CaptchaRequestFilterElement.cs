using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "captchaRequestFilter" element
    public class CaptchaRequestFilterElement : ConfigurationElement
    {
        // Create a "enabled" attribute.
        [ConfigurationProperty("enabled", DefaultValue = CaptchaDefaults.RequestFilterEnabled, IsRequired = false)]
        public Boolean Enabled
        {
            get
            {
                return (Boolean)this["enabled"];
            }
        }

        // Create a "allowedRepeatedRequests" element.
        [ConfigurationProperty("allowedRepeatedRequests", DefaultValue = CaptchaDefaults.AllowedRepeatedRequests, IsRequired = false)]
        [IntegerValidator(ExcludeRange = false, MinValue = CaptchaDefaults.MinAllowedRepeatedRequests, MaxValue = CaptchaDefaults.MaxAllowedRepeatedRequests)]
        public int AllowedRepeatedRequests
        {
            get
            {
                return (int)this["allowedRepeatedRequests"];
            }
        }
    }
}
