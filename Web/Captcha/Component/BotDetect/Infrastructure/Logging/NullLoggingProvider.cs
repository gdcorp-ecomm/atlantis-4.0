using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Logging
{
    /// <summary>
    /// The default logging provider used if no other provider is configured, 
    /// NullLoggingProvider simply does nothing with the captured events.
    /// </summary>
    public class NullLoggingProvider : ILoggingProvider
    {
        // singleton
        private static readonly NullLoggingProvider _instance = new NullLoggingProvider();
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static NullLoggingProvider Instance
        {
            get
            {
                return _instance;
            }
        }

        private NullLoggingProvider()
        {
        }

        /// <summary>
        /// Is error logging enabled
        /// </summary>
        public bool ErrorLoggingEnabled
        {
            get
            {
                return true;
            }

            set
            {
                // do nothing - send to null
            }
        }

        /// <summary>
        /// Is debug logging enabled
        /// </summary>
        public bool TraceEnabled
        {
            get
            {
                return true;
            }

            set
            {
                // do nothing - send to null
            }
        }

        /// <summary>
        /// Log the provided input to the BotDetect error log
        /// </summary>
        /// <param name="e"></param>
        public void LogError(Exception e)
        {
            // do nothing - send to null
        }

        /// <summary>
        /// Log the provided input to the BotDetect error log
        /// </summary>
        /// <param name="values"></param>
        public void LogError(params object[] values)
        {
            // do nothing - send to null
        }

        /// <summary>
        /// Log the provided input to the BotDetect trace / debug log
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="values"></param>
        public void Trace(string eventType, params object[] values)
        {
            // do nothing - send to null
        }

        /// <summary>
        /// Filtered tracing, only logging events whose eventType matches this regex
        /// </summary>
        public string EventTypeFilterRegex
        {
            get
            {
                return null;
            }

            set
            {
                // do nothing - send to null
            }
        }
    }
}
