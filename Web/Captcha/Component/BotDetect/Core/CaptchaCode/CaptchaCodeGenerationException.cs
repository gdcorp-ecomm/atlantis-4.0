using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect.CaptchaCode
{
    [Serializable]
    public class CaptchaCodeGenerationException : CaptchaGenerationException
    {
        const string message = "Captcha code generation error";

        public CaptchaCodeGenerationException( ) :
            base(message)
        {
        }

        public CaptchaCodeGenerationException( string auxMessage ) :
            base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

        public CaptchaCodeGenerationException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public CaptchaCodeGenerationException(string auxMessage, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public CaptchaCodeGenerationException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected CaptchaCodeGenerationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
