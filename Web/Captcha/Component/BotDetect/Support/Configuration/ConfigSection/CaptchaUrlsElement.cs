using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "captchaUrls" element
    public class CaptchaUrlsElement : ConfigurationElement
    {
        // Create a "requestPath" element.
        [ConfigurationProperty("requestPath", DefaultValue = CaptchaDefaults.RequestPath, IsRequired = false)]
        public String RequestPath
        {
            get
            {
                return (String)this["requestPath"];
            }
        }
    }
}
