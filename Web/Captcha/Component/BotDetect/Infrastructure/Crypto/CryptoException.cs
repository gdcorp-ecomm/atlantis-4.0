using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

namespace BotDetect
{
    [Serializable]
    internal class CryptoException : BaseException
    {
        const string message = "An error occured in the BotDetect.Crypto wrapper";

        public CryptoException() 
            : base(message)
        {
        }

        public CryptoException(string auxMessage)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage))
        {
        }

       public CryptoException(string auxMessage, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), values)
        {
        }

        public CryptoException(string auxMessage, Exception inner)
           : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner)
        { 
        }

        public CryptoException(string auxMessage, Exception inner, params object[] values)
            : base(String.Format(CultureInfo.InvariantCulture, "{0} - {1}", message, auxMessage), inner, values)
        {
        }

        protected CryptoException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
