using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect.Logging
{
    [Serializable]
    public class LoggingException : BaseException
    {
        const string message = "An error occured in the BotDetect.Logging wrapper";

        public LoggingException() 
            : base(message)
        {
        }

        public LoggingException(string auxMessage)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

        public LoggingException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public LoggingException(string auxMessage, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public LoggingException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected LoggingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
