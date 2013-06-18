using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect
{
    [Serializable]
    public class MathException : BaseException
    {
        const string message = "An error occured in the BotDetect.Math wrapper";

        public MathException() 
            : base(message)
        {
        }

        public MathException(string auxMessage)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

        public MathException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public MathException(string auxMessage, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public MathException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected MathException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
