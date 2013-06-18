using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect.CaptchaImage
{
    [Serializable]
    public class CaptchaImageGenerationException : CaptchaGenerationException
    {
        const string message = "Captcha image generation error";

        public CaptchaImageGenerationException( ) :
            base(message)
        {
        }

        public CaptchaImageGenerationException( string auxMessage ) :
            base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

        public CaptchaImageGenerationException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public CaptchaImageGenerationException(string auxMessage, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public CaptchaImageGenerationException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected CaptchaImageGenerationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
