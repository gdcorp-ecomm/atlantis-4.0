using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    public class CaptchaEncryptionConfiguration : ICaptchaEncryptionConfiguration
    {
        // singleton
        private CaptchaEncryptionConfiguration()
        {
        }

        private static readonly CaptchaEncryptionConfiguration _instance = new CaptchaEncryptionConfiguration();

        public static CaptchaEncryptionConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public string EncryptionPassword
        {
            get 
            {
                string password = CaptchaDefaults.EncryptionPassword;

                if (null != CaptchaConfiguration.ConfigurationProvider && 
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaEncryption)
                {
                    string configuredPassword = CaptchaConfiguration.ConfigurationProvider.CaptchaEncryption.EncryptionPassword;
                    if (StringHelper.HasValue(configuredPassword))
                    {
                        password = configuredPassword;
                    }
                }

                return password;
            }
        }

    }
}
