using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    public class CaptchaUrlConfiguration : ICaptchaUrlConfiguration
    {
        // singleton
        private CaptchaUrlConfiguration()
        {
        }

        private static readonly CaptchaUrlConfiguration _instance = new CaptchaUrlConfiguration();

        public static CaptchaUrlConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public String RequestPath
        {
            get
            {
                string requestPath = CaptchaDefaults.RequestPath;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaUrls)
                {
                    string configuredPath = CaptchaConfiguration.ConfigurationProvider.CaptchaUrls.RequestPath;
                    if (StringHelper.HasValue(configuredPath))
                    {
                        requestPath = configuredPath;
                    }
                }

                return requestPath;
            }
        }
    }
}
