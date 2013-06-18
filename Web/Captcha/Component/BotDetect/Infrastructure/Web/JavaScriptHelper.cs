using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect
{
    public sealed class JavaScriptHelper
    {
        private JavaScriptHelper() {}

        public static string Boolean(bool value)
        {
            return value.ToString().ToLowerInvariant();
        }

        public static string String(string value)
        {
            if (StringHelper.HasValue(value))
            {
                return ("'" + value + "'");
            }
            else
            {
                return "null";
            }
        }
    }
}
