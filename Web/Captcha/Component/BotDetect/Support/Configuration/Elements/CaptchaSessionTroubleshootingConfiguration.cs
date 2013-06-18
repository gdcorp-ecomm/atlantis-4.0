using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    public class CaptchaSessionTroubleshootingConfiguration : ICaptchaSessionTroubleshootingConfiguration
    {
        // singleton
        private CaptchaSessionTroubleshootingConfiguration()
        {
        }

        private static readonly CaptchaSessionTroubleshootingConfiguration _instance = new CaptchaSessionTroubleshootingConfiguration();

        public static CaptchaSessionTroubleshootingConfiguration Instance
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
                bool enabled = CaptchaDefaults.CaptchaSessionTroubleshootingEnabled;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaSessionTroubleshooting)
                {
                    bool configuredFlag = CaptchaConfiguration.ConfigurationProvider.CaptchaSessionTroubleshooting.Enabled;
                    enabled = configuredFlag;
                }

                return enabled;
            }
        }
    }
}
