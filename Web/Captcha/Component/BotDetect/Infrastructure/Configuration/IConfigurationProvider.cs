using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Configuration
{
    internal interface IConfigurationProvider
    {
        object this[string key] { get; }
    }
}
