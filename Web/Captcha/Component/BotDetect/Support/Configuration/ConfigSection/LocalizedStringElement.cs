using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "localizedString" type
    public class LocalizedStringElement : ConfigurationElement
    {
        // Define the "locale" attribute
        [ConfigurationProperty("locale", IsRequired = false, IsKey = true)]
        public String Locale
        {
            get
            {
                return (String)this["locale"];
            }
        }

        // Define the "value" attribute
        [ConfigurationProperty("value", IsRequired = true, IsKey = true)]
        public String Value
        {
            get
            {
                return (String)this["value"];
            }
        }
        
    }    
}
