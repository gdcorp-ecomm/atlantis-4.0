using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace BotDetect.Logging
{
    internal sealed class LoggingProviderHelper
    {
        private LoggingProviderHelper()
        {
        }

        public static ILoggingProvider LoadProvider(string providerTypeName)
        {
            // default - null - logging provider
            if(0 == String.Compare(CaptchaDefaults.LoggingProvider, providerTypeName, StringComparison.Ordinal))
            {
                return NullLoggingProvider.Instance;
            }

            // load the specified type from the specified assembly
            Type providerType = Type.GetType(providerTypeName, false);

            // ILooggingProvider has to have this property
            PropertyInfo logProperty = providerType.GetProperty("Instance",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);

            // static so no object reference, and not an indexer so no index
            ILoggingProvider _log = logProperty.GetValue(null, null) as ILoggingProvider;

            return _log;
        }
    }
}
