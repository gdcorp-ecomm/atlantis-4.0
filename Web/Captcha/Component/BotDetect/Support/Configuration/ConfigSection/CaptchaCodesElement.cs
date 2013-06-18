using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "captchaCodes" element
    public class CaptchaCodesElement : ConfigurationElement
    {
        // Define the "timeout" attribute
        [ConfigurationProperty("timeout", DefaultValue = 1200, IsRequired = false)]
        public int Timeout
        {
            get
            {
                return (int)this["timeout"];
            }
        }

        // Define the  "characterSets" sub-element collection
        [ConfigurationProperty("characterSets", IsRequired = false, IsDefaultCollection = false)]
        public CharacterSetElementCollection CharacterSets
        {
            get
            {
                return (CharacterSetElementCollection)this["characterSets"];
            }
        }

        // Define the "testMode" sub-element
        [ConfigurationProperty("testMode", IsRequired = false)]
        public TestModeElement TestMode
        {
            get
            {
                return (TestModeElement)this["testMode"];
            }
        }
    }
}
