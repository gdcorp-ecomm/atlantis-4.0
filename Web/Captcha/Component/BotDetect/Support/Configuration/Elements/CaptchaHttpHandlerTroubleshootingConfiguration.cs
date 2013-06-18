using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    public class CaptchaHttpHandlerTroubleshootingConfiguration : ICaptchaHttpHandlerTroubleshootingConfiguration
    {
        // singleton
        private CaptchaHttpHandlerTroubleshootingConfiguration()
        {
        }

        private static readonly CaptchaHttpHandlerTroubleshootingConfiguration _instance = new CaptchaHttpHandlerTroubleshootingConfiguration();

        public static CaptchaHttpHandlerTroubleshootingConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public Boolean Enabled
        {
            get
            {
                bool enabled = CaptchaDefaults.CaptchaHttpHandlerTroubleshootingEnabled;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaHttpHandlerTroubleshooting)
                {
                    bool configuredFlag = CaptchaConfiguration.ConfigurationProvider.CaptchaHttpHandlerTroubleshooting.Enabled;
                    enabled = configuredFlag;
                }

                return enabled;
            }
        }
    }
}
