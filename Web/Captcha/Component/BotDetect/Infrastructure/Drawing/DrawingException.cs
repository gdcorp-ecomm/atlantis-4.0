using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect.Drawing
{
    [Serializable]
    public class DrawingException : BaseException
    {
        const string message = "An error occured in the BotDetect.Drawing wrapper";

        public DrawingException() 
            : base(message)
        {
        }

        public DrawingException(string auxMessage)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

        public DrawingException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public DrawingException(string auxMessage, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public DrawingException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected DrawingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
