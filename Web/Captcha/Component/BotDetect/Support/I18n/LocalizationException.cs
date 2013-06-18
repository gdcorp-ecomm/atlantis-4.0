using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect
{
    [Serializable]
    public class LocalizationException : BaseException
    {
        const string message = "An error occured in the BotDetect localization code";

        public LocalizationException() 
            : base(message)
        {
        }

        public LocalizationException(string auxMessage)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

        public LocalizationException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public LocalizationException(string auxMessage, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public LocalizationException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected LocalizationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
