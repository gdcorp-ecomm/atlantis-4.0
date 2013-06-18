using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using BotDetect.Logging;

namespace BotDetect
{
    public class CaptchaLogging
    {
        // singleton
        private CaptchaLogging()
        {
        }

        private static readonly CaptchaLogging _instance = new CaptchaLogging();

        public static CaptchaLogging Instance
        {
            get
            {
                return _instance;
            }
        }

        private static ILoggingProvider _log;
        internal static ILoggingProvider LoggingProvider
        {
            get
            {
                if (null == _log)
                {
                    // lazy initialization of the configured logging provider
                    string providerTypeName = CaptchaConfiguration.CaptchaLogging.LoggingProvider;
                    _log = LoggingProviderHelper.LoadProvider(providerTypeName);

                    // set properties
                    _log.ErrorLoggingEnabled = CaptchaConfiguration.CaptchaLogging.ErrorLoggingEnabled;
                    _log.TraceEnabled = CaptchaConfiguration.CaptchaLogging.TraceEnabled;
                    _log.EventTypeFilterRegex = CaptchaConfiguration.CaptchaLogging.EventFilter;
                }

                return _log;
            }
        }

        public static bool ErrorLoggingEnabled
        {
            get
            {
                return LoggingProvider.ErrorLoggingEnabled;
            }
        }

        public static bool TraceEnabled
        {
            get
            {
                return LoggingProvider.TraceEnabled;
            }
        }

        public static void LogError(Exception e)
        {
            LoggingProvider.LogError(e);
        }

        public static void LogError(params object[] values)
        {
            LoggingProvider.LogError(values);
        }

        public static void Trace(string eventType, params object[] values)
        {
            LoggingProvider.Trace(eventType, values);
        }

        public static string EventTypeFilterRegex
        {
            get
            {
                return LoggingProvider.EventTypeFilterRegex;
            }
        }
    }
}
