using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect
{
    [Serializable]
    public abstract class BaseException : System.Exception
    {
        protected BaseException()
            : base()
        {
        }

        protected BaseException(string message)
            :base(message)
        {
        }

        protected BaseException(string message, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, StringHelper.LogFriendly(values)))
        {
        }

        protected BaseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected BaseException(string message, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, StringHelper.LogFriendly(values)), inner)
        {
        }

        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override string ToString()
        {
            return "BotDetect.BaseException {\r\n" + base.ToString() + "\r\n}";
        }
    }
}
