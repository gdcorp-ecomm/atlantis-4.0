using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect
{
    [Serializable]
    public class CaptchaControlException : CaptchaException
    {
        const string message = "Captcha web control error";

        public CaptchaControlException( ) 
            : base(message)
        {
        }

        public CaptchaControlException( string auxMessage )
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

        public CaptchaControlException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public CaptchaControlException(string auxMessage, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public CaptchaControlException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected CaptchaControlException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
