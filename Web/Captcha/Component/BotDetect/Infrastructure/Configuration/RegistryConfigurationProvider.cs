using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Configuration
{
    internal class RegistryConfigurationProvider
    {
        // singleton
        private static readonly RegistryConfigurationProvider _instance = new RegistryConfigurationProvider();
        public static RegistryConfigurationProvider Configuration
        {
            get
            {
                return _instance;
            }
        }

        private RegistryConfigurationProvider()
        {
        }

        // indexer
        public object this[string key]
        {
            get
            {
                throw new NotImplementedException("Windows Registry reading not implemented");
            }
        }
    }
}
