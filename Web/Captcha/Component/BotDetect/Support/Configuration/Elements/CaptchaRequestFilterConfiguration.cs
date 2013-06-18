using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    public class CaptchaRequestFilterConfiguration : ICaptchaRequestFilterConfiguration
    {
        // singleton
        private CaptchaRequestFilterConfiguration()
        {
        }

        private static readonly CaptchaRequestFilterConfiguration _instance = new CaptchaRequestFilterConfiguration();

        public static CaptchaRequestFilterConfiguration Instance
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
                bool enabled = CaptchaDefaults.RequestFilterEnabled;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaRequestFilter)
                {
                    bool configuredFlag = CaptchaConfiguration.ConfigurationProvider.CaptchaRequestFilter.Enabled;
                    enabled = configuredFlag;
                }

                return enabled;
            }
        }

        public int AllowedRepeatedRequests
        {
            get
            {
                int allowedRepeatedRequests = CaptchaDefaults.AllowedRepeatedRequests;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaRequestFilter)
                {
                    int configuredCount = CaptchaConfiguration.ConfigurationProvider.CaptchaRequestFilter.AllowedRepeatedRequests;
                    allowedRepeatedRequests = configuredCount;
                }

                return allowedRepeatedRequests;
            }
        }
    }
}
