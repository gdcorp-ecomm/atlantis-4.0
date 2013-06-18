using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    public class CaptchaUserInputElement : ConfigurationElement
    {
        // Define the "autoUppercase" attribute
        [ConfigurationProperty("autoUppercase", DefaultValue = CaptchaDefaults.AutoUppercaseInput, IsRequired = false)]
        public Boolean AutoUppercase
        {
            get
            {
                return (Boolean)this["autoUppercase"];
            }
        }

        // Define the "autoLowercase" attribute (backwards compatibility only)
        [ConfigurationProperty("autoLowercase", DefaultValue = true, IsRequired = false)]
        public Boolean AutoLowercase
        {
            get
            {
                return (Boolean)this["autoLowercase"];
            }
        }


        // Define the "autoClear" attribute
        [ConfigurationProperty("autoClear", DefaultValue = CaptchaDefaults.AutoClearInput, IsRequired = false)]
        public Boolean AutoClear
        {
            get
            {
                return (Boolean)this["autoClear"];
            }
        }

        // Define the "autoFocus" attribute
        [ConfigurationProperty("autoFocus", DefaultValue = CaptchaDefaults.AutoFocusInput, IsRequired = false)]
        public Boolean AutoFocus
        {
            get
            {
                return (Boolean)this["autoFocus"];
            }
        }

    }
}
