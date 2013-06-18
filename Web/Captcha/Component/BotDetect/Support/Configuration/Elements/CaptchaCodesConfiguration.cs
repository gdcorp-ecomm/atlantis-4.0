using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace BotDetect.Configuration
{
    public class CaptchaCodesConfiguration : ICaptchaCodesConfiguration
    {
        // singleton
        private CaptchaCodesConfiguration() {}

        private static readonly CaptchaCodesConfiguration _instance = new CaptchaCodesConfiguration();

        public static CaptchaCodesConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public int Timeout
        {
            get
            {
                int timeout = CaptchaDefaults.CodeTimeout;

                // use the configured timeout value
                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaCodes)
                {
                    int configuredTimeout = CaptchaConfiguration.ConfigurationProvider.CaptchaCodes.Timeout;
                    timeout = configuredTimeout;
                }

                // if the timeout is set to the default value (=20 minutes), 
                // replace it with the current Session timeout
                if ((timeout == CaptchaDefaults.CodeTimeout) &&
                    (null != HttpContext.Current) &&
                    (null != HttpContext.Current.Session))
                {
                    timeout = HttpContext.Current.Session.Timeout * 60;
                }

                return timeout;
            }
        }

        public ICharacterSetCollectionConfiguration CharacterSets
        {
            get
            {
                return CharacterSetsConfiguration.Instance as ICharacterSetCollectionConfiguration;
            }
        }

        public ITestModeConfiguration TestMode
        {
            get
            {
                return TestModeConfiguration.Instance as ITestModeConfiguration;
            }
        }
    }

    public class CharacterSetsConfiguration : ICharacterSetCollectionConfiguration
    {
        // singleton
        private CharacterSetsConfiguration()
        {
        }

        private static readonly CharacterSetsConfiguration _instance = new CharacterSetsConfiguration();

        public static CharacterSetsConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public ICharacterSetConfiguration this[string name]
        {
            get
            {
                ICharacterSetConfiguration customCharacterSet = null;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaCodes &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaCodes.CharacterSets)
                {
                    CharacterSetElement configuredCharset = CaptchaConfiguration.ConfigurationProvider.CaptchaCodes.CharacterSets[name];
                    customCharacterSet = configuredCharset as ICharacterSetConfiguration;
                }

                return customCharacterSet;
            }
        }
    }

    public class TestModeConfiguration : ITestModeConfiguration
    {
        // singleton
        private TestModeConfiguration()
        {
        }

        private static readonly TestModeConfiguration _instance = new TestModeConfiguration();

        public static TestModeConfiguration Instance
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
                bool enabled = CaptchaDefaults.TestModeEnabled;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaCodes &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaCodes.TestMode)
                {
                    bool configuredFlag = CaptchaConfiguration.ConfigurationProvider.CaptchaCodes.TestMode.Enabled;
                    enabled = configuredFlag;
                }

                return enabled;
            }
        }
    }
}
