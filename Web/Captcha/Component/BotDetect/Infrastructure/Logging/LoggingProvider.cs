using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BotDetect.Logging
{
    /// <summary>
    /// The base logging provider class. Inheriting this class makes 
    /// fulfilling the ILoggingProvider interface requirements as 
    /// simple as implementing two methods logging strings to the 
    /// error and trace logs.
    /// </summary>
    public abstract class LoggingProvider : ILoggingProvider
    {
        private bool _logErrors;
        /// <summary>
        /// Is error logging enabled
        /// </summary>
        public bool ErrorLoggingEnabled
        {
            get
            {
                return _logErrors;
            }

            set
            {
                _logErrors = value;
            }
        }

        private bool _logTraces;
        /// <summary>
        /// Is debug logging enabled
        /// </summary>
        public bool TraceEnabled
        {
            get
            {
                return _logTraces;
            }

            set
            {
                _logTraces = value;
            }
        }

        /// <summary>
        /// Log the provided input to the BotDetect error log
        /// </summary>
        /// <param name="e"></param>
        public virtual void LogError(Exception e)
        {
            if (_logErrors)
            {
                this.LogError("ERROR", StringHelper.ToString(e));
            }
        }

        /// <summary>
        /// Log the provided input to the BotDetect error log
        /// </summary>
        /// <param name="values"></param>
        public virtual void LogError(params object[] values)
        {
            if (_logErrors)
            {
                this.LogError("ERROR", values);
            }
        }

        /// <summary>
        /// Log the provided input to the BotDetect error log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public virtual void LogError(string message, params object[] values)
        {
            StringBuilder builder = new StringBuilder(message);
            if (null != values && 0 != values.Length)
            {
                // write all values, comma-separated
                string[] stringValues = StringHelper.GetStringArray(values);
                int end = values.Length - 1;
                for (int i = 0; i < end; i++)
                {
                    builder.Append(StringHelper.LogFriendly(values[i]));
                    builder.AppendLine();
                }
                builder.Append(StringHelper.LogFriendly(values[end]));
            }
            this.LogError(builder.ToString());
            builder.Length = 0;
        }

        /// <summary>
        /// Log the provided input to the BotDetect error log
        /// </summary>
        /// <param name="value"></param>
        public abstract void LogError(string value);

        /// <summary>
        /// Log the provided input to the BotDetect trace / debug log
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="values"></param>
        public void Trace(string eventType, params object[] values)
        {
            if (_logTraces && _includedEventTypes.IsMatch(eventType))
            {
                StringBuilder builder = new StringBuilder("DEBUG ");
                builder.Append(eventType);
                if (null != values && 0 != values.Length)
                {
                    // write all values, comma-separated
                    string[] stringValues = StringHelper.GetStringArray(values);
                    int end = values.Length - 1;
                    for (int i = 0; i < end; i++)
                    {
                        builder.Append(StringHelper.LogFriendly(values[i]));
                        builder.AppendLine();
                    }
                    builder.Append(StringHelper.LogFriendly(values[end]));
                }
                this.Trace(builder.ToString());
                builder.Length = 0;
            }
        }

        /// <summary>
        /// Log the provided input to the BotDetect trace / debug log
        /// </summary>
        /// <param name="value"></param>
        public abstract void Trace(string value);

        // debug logging filter
        private static Regex _includedEventTypes = new Regex(".*", 
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        private static readonly object _regexLocker = new object();

        /// <summary>
        /// Filtered tracing, only logging events whose eventType matches this regex
        /// </summary>
        public string EventTypeFilterRegex
        {
            get
            {
                return StringHelper.ToString(_includedEventTypes);
            }

            set
            {
                if (0 == String.Compare(value, 
                    StringHelper.ToString(_includedEventTypes), 
                    StringComparison.Ordinal))
                {
                    // skip making a new regex for the exact same pattern
                    return;
                }

                // construct new regex
                lock (_regexLocker)
                {
                    // in case the lock was not acquired immediately 
                    // and another thread already updated the reference
                    if (0 == String.Compare(value, 
                        StringHelper.ToString(_includedEventTypes), 
                        StringComparison.Ordinal))
                    {
                        Regex newRegex = new Regex(value, 
                            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
										
                        _includedEventTypes = newRegex;
                    }
                }
            }
        }
    }
}
