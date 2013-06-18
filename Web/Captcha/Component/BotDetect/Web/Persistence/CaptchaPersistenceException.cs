using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect.Web
{
    [Serializable]
    public class CaptchaPersistenceException : CaptchaException
    {
        const string message = "Captcha persistence error";

        public CaptchaPersistenceException( ) :
            base(message)
        {
        }

        public CaptchaPersistenceException( string auxMessage ) :
            base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

        public CaptchaPersistenceException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public CaptchaPersistenceException(string auxMessage, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public CaptchaPersistenceException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected CaptchaPersistenceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
