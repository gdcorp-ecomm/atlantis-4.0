using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect.CaptchaSound
{
    [Serializable]
    public class CaptchaSoundGenerationException : CaptchaGenerationException
    {
        const string message = "Captcha sound generation error";

        public CaptchaSoundGenerationException( ) :
            base(message)
        {
        }

        public CaptchaSoundGenerationException( string auxMessage ) :
            base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

        public CaptchaSoundGenerationException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public CaptchaSoundGenerationException(string auxMessage, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public CaptchaSoundGenerationException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected CaptchaSoundGenerationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
