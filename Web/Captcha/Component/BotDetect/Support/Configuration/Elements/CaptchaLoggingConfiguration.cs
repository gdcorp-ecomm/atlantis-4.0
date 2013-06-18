using System;
using System.Collections;
using System.Text;

namespace BotDetect.Configuration
{
    public class CaptchaLoggingConfiguration : ICaptchaLoggingConfiguration
    {
        // singleton
        private CaptchaLoggingConfiguration()
        {
        }

        private static readonly CaptchaLoggingConfiguration _instance = new CaptchaLoggingConfiguration();

        public static CaptchaLoggingConfiguration Instance
        {
            get
            {
                return _instance;
            }
        }

        public bool ErrorLoggingEnabled
        {
            get
            {
                bool logErrors = CaptchaDefaults.ErrorLoggingEnabled;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaLogging)
                {
                    bool configuredFlag = CaptchaConfiguration.ConfigurationProvider.CaptchaLogging.ErrorLoggingEnabled;
                    logErrors = configuredFlag;
                }

                return logErrors;
            }
        }

        public bool TraceEnabled
        {
            get
            {
                bool logTraces = CaptchaDefaults.TraceEnabled;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaLogging)
                {
                    bool configuredFlag = CaptchaConfiguration.ConfigurationProvider.CaptchaLogging.TraceEnabled;
                    logTraces = configuredFlag;
                }

                return logTraces;
            }
        }

        public string EventFilter
        {
            get
            {
                string eventFilter = CaptchaDefaults.EventFilter;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaLogging)
                {
                    string configuredFilter = CaptchaConfiguration.ConfigurationProvider.CaptchaLogging.EventFilter;
                    if (StringHelper.HasValue(configuredFilter))
                    {
                        eventFilter = configuredFilter;
                    }
                }

                return eventFilter;
            }
        }

        public string LoggingProvider
        {
            get
            {
                string loggingProviderType = CaptchaDefaults.LoggingProvider;

                if (null != CaptchaConfiguration.ConfigurationProvider &&
                    null != CaptchaConfiguration.ConfigurationProvider.CaptchaLogging)
                {
                    string configuredType = CaptchaConfiguration.ConfigurationProvider.CaptchaLogging.LoggingProvider;

                    if (StringHelper.HasValue(configuredType))
                    {
                        loggingProviderType = configuredType;
                    }
                }

                return loggingProviderType;
            }
        }
    }
}
