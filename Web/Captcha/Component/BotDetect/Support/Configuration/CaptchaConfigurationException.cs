using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect.Configuration
{
    [Serializable]
    public class CaptchaConfigurationException : CaptchaException
    {
        const string message = "Captcha configuration error";

        public CaptchaConfigurationException( ) :
            base(message)
        {
        }

        public CaptchaConfigurationException( string auxMessage ) :
            base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

        public CaptchaConfigurationException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public CaptchaConfigurationException(string auxMessage, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public CaptchaConfigurationException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected CaptchaConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
