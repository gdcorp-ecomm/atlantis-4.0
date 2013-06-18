using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect
{
    [Serializable]
    public class CaptchaGenerationException : CaptchaException
    {
        const string message = "Captcha generation error";

        public CaptchaGenerationException( ) :
            base(message)
        {
        }

        public CaptchaGenerationException( string auxMessage ) :
            base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

        public CaptchaGenerationException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public CaptchaGenerationException(string auxMessage, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public CaptchaGenerationException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected CaptchaGenerationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
