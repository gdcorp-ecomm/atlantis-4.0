using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Logging
{
    /// <summary>
    /// Every logging provider used for BotDetect logging should implement this interface.
    /// </summary>
    public interface ILoggingProvider
    {
        /// <summary>
        /// Is error logging enabled
        /// </summary>
        bool ErrorLoggingEnabled { get; set; }

        /// <summary>
        /// Log the provided input to the BotDetect error log
        /// </summary>
        /// <param name="e"></param>
        void LogError(Exception e);

        /// <summary>
        /// Log the provided input to the BotDetect error log
        /// </summary>
        /// <param name="values"></param>
        void LogError(params object[] values);

        /// <summary>
        /// Is debug logging enabled
        /// </summary>
        bool TraceEnabled { get; set; }

        /// <summary>
        /// Filtered tracing, only logging events whose eventType matches this regex
        /// </summary>
        string EventTypeFilterRegex { get; set; }

        /// <summary>
        /// Log the provided input to the BotDetect trace / debug log
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="values"></param>
        void Trace(string eventType, params object[] values);
    }
}
