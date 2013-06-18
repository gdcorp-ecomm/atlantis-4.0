using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Configuration
{
    internal class IniFileConfigurationProvider : IConfigurationProvider
    {
        // singleton
        private static readonly IniFileConfigurationProvider _instance = new IniFileConfigurationProvider();
        public static IniFileConfigurationProvider Configuration
        {
            get
            {
                return _instance;
            }
        }

        private IniFileConfigurationProvider()
        {
        }

        // indexer
        public object this[string key]
        {
            get
            {
                throw new NotImplementedException(".ini file parsing not implemented");
            }
        }
    }
}
