using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "encryption" element
    public class CaptchaEncryptionElement : ConfigurationElement
    {
        // Create a "encryptionPassword" element.
        [ConfigurationProperty("encryptionPassword", IsRequired = false)]
        public String EncryptionPassword
        {
            get
            {
                return (String)this["encryptionPassword"];
            }
        }
    }
}
