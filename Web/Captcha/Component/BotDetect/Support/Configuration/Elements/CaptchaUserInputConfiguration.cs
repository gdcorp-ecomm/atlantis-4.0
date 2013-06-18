using System;
using System.Collections;
using System.Text;

namespace BotDetect.Configuration
{
    public class CaptchaUserInputConfiguration : ICaptchaUserInputConfiguration
    {
        // singleton
        private CaptchaUserInputConfiguration()
        {
        }

        private static readonly CaptchaUserInputConfiguration _instance = new CaptchaUserInputConfiguration();

        public static CaptchaUserInputConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public bool AutoUppercase
        {
            get
            {
                bool enabled = CaptchaDefaults.AutoUppercaseInput;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaUserInput)
                {
                    bool configuredFlag = CaptchaConfiguration.ConfigurationProvider.CaptchaUserInput.AutoUppercase;

                    // to provide backwards compatibility with the old "autoLowercase" setting while implementing
                    // the new default, we check if the old functionality was turned off 
                    bool oldSetting = CaptchaConfiguration.ConfigurationProvider.CaptchaUserInput.AutoLowercase;

                    // - only one setting is allowed
                    // - if there is no configuration override, return true
                    // - if the new setting is used, return its value
                    // - if the old setting is used, return its value
                    enabled = (configuredFlag && oldSetting);
                }

                return enabled;
            }
        }

        public bool AutoClear
        {
            get
            {
                bool enabled = CaptchaDefaults.AutoClearInput;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaUserInput)
                {
                    bool configuredFlag = CaptchaConfiguration.ConfigurationProvider.CaptchaUserInput.AutoClear;
                    enabled = configuredFlag;
                }

                return enabled;
            }
        }

        public bool AutoFocus
        {
            get
            {
                bool enabled = CaptchaDefaults.AutoFocusInput;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaUserInput)
                {
                    bool configuredFlag = CaptchaConfiguration.ConfigurationProvider.CaptchaUserInput.AutoFocus;
                    enabled = configuredFlag;
                }

                return enabled;
            }
        }
    }
}
