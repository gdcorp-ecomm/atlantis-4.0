using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

using BotDetect.Configuration;

namespace BotDetect
{
    public sealed class CaptchaConfiguration
    {
        // configuration provider
        internal static BotDetectConfigurationSection ConfigurationProvider
        {
            get
            {
                return ConfigurationManager.GetSection("botDetect") as BotDetectConfigurationSection;
            }
        }
       
        // singleton
        private CaptchaConfiguration()
        {
        }

        private static readonly CaptchaConfiguration _instance = new CaptchaConfiguration();

        public static CaptchaConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        // ICaptchaConfiguration Members
        public static ICaptchaCodesConfiguration CaptchaCodes
        {
            get 
            {
                return CaptchaCodesConfiguration.Instance;
            }
        }

        public static ICaptchaImageConfiguration CaptchaImage
        {
            get
            {
                return CaptchaImageConfiguration.Instance;
            }
        }

        public static ICaptchaSoundConfiguration CaptchaSound
        {
            get
            {
                return CaptchaSoundConfiguration.Instance;
            }
        }

        public static ICaptchaReloadingConfiguration CaptchaReloading
        {
            get
            {
                return CaptchaReloadingConfiguration.Instance;
            }
        }

        public static ICaptchaUserInputConfiguration CaptchaUserInput
        {
            get
            {
                return CaptchaUserInputConfiguration.Instance;
            }
        }

        public static ICaptchaLoggingConfiguration CaptchaLogging
        {
            get
            {
                return CaptchaLoggingConfiguration.Instance;
            }
        }

        public static ICaptchaUrlConfiguration CaptchaUrls
        {
            get
            {
                return CaptchaUrlConfiguration.Instance;
            }
        }

        public static ICaptchaEncryptionConfiguration CaptchaEncryption
        {
            get
            {
                return CaptchaEncryptionConfiguration.Instance;
            }
        }

        public static ICaptchaRequestFilterConfiguration CaptchaRequestFilter
        {
            get
            {
                return CaptchaRequestFilterConfiguration.Instance;
            }
        }

        public static ICaptchaHttpHandlerTroubleshootingConfiguration CaptchaHttpHandlerTroubleshooting
        {
            get
            {
                return CaptchaHttpHandlerTroubleshootingConfiguration.Instance;
            }
        }

        public static ICaptchaSessionTroubleshootingConfiguration CaptchaSessionTroubleshooting
        {
            get
            {
                return CaptchaSessionTroubleshootingConfiguration.Instance;
            }
        }
    }
}
