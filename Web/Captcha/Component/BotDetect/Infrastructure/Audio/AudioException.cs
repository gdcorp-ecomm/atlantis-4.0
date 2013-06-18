using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect.Audio
{
    [Serializable]
    public class AudioException : BaseException
    {
        const string message = "An error occured in the BotDetect.Audio wrapper";

        public AudioException() 
            : base(message)
        {
        }

        public AudioException(string auxMessage) 
            : base( String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

        public AudioException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public AudioException(string auxMessage, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public AudioException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected AudioException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
