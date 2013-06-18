using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    internal class DotNetConfigurationProvider : IConfigurationProvider
    {
        // singleton
        private static readonly DotNetConfigurationProvider _instance = new DotNetConfigurationProvider();
        public static DotNetConfigurationProvider Configuration
        {
            get
            {
                return _instance;
            }
        }

        private DotNetConfigurationProvider()
        {
        }

        // indexer
        public object this[string key]
        {
            get
            {
                return ConfigurationManager.AppSettings[key];
            }
        }
    }
}
