using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

namespace BotDetect.Configuration
{
    // Define the "disabledSoundStyles" element
    public class DisabledSoundStylesElement : ConfigurationElement
    {
        public StringCollection Names
        {
            get
            {
                return StringHelper.ParseCsv(this.RawNames);
            }
        }

        // Define the "Names" attribute
        [ConfigurationProperty("names", IsRequired = false)]
        public String RawNames
        {
            get
            {
                return (String)this["names"];
            }
        }
    }
}
