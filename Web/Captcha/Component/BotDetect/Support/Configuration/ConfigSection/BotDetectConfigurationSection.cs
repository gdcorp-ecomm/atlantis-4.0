using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    public class BotDetectConfigurationSection : ConfigurationSection
    {
        // Define the "captchaCodes" element
        [ConfigurationProperty("captchaCodes", IsRequired = false)]
        public CaptchaCodesElement CaptchaCodes
        {
            get
            {
                return (CaptchaCodesElement)this["captchaCodes"];
            }
        }

        // Define the "captchaSound" element
        [ConfigurationProperty("captchaSound", IsRequired = false)]
        public CaptchaSoundElement CaptchaSound
        {
            get
            {
                return (CaptchaSoundElement)this["captchaSound"];
            }
        }

        // Define the "captchaImage" element
        [ConfigurationProperty("captchaImage", IsRequired = false)]
        public CaptchaImageElement CaptchaImage
        {
            get
            {
                return (CaptchaImageElement)this["captchaImage"];
            }
        }

        // Define the "captchaReloading" element
        [ConfigurationProperty("captchaReloading", IsRequired = false)]
        public CaptchaReloadingElement CaptchaReloading
        {
            get
            {
                return (CaptchaReloadingElement)this["captchaReloading"];
            }
        }

        // Define the "captchaUserInput" element
        [ConfigurationProperty("captchaUserInput", IsRequired = false)]
        public CaptchaUserInputElement CaptchaUserInput
        {
            get
            {
                return (CaptchaUserInputElement)this["captchaUserInput"];
            }
        }

        // Define the "captchaLogging" element
        [ConfigurationProperty("captchaLogging", IsRequired = false)]
        public CaptchaLoggingElement CaptchaLogging
        {
            get
            {
                return (CaptchaLoggingElement)this["captchaLogging"];
            }
        }

        // Define the "captchaUrls" element
        [ConfigurationProperty("captchaUrls", IsRequired = false)]
        public CaptchaUrlsElement CaptchaUrls
        {
            get
            {
                return (CaptchaUrlsElement)this["captchaUrls"];
            }
        }

        // Define the "captchaEncryption" element
        [ConfigurationProperty("captchaEncryption", IsRequired = false)]
        public CaptchaEncryptionElement CaptchaEncryption
        {
            get
            {
                return (CaptchaEncryptionElement)this["captchaEncryption"];
            }
        }

        // Define the "captchaRequestFilter" element
        [ConfigurationProperty("captchaRequestFilter", IsRequired = false)]
        public CaptchaRequestFilterElement CaptchaRequestFilter
        {
            get
            {
                return (CaptchaRequestFilterElement)this["captchaRequestFilter"];
            }
        }

        // Define the "captchaSessionTroubleshooting" element
        [ConfigurationProperty("captchaSessionTroubleshooting", IsRequired = false)]
        public CaptchaSessionTroubleshootingElement CaptchaSessionTroubleshooting
        {
            get
            {
                return (CaptchaSessionTroubleshootingElement)this["captchaSessionTroubleshooting"];
            }
        }

        // Define the "captchaHttpHandlerTroubleshooting" element
        [ConfigurationProperty("captchaHttpHandlerTroubleshooting", IsRequired = false)]
        public CaptchaHttpHandlerTroubleshootingElement CaptchaHttpHandlerTroubleshooting
        {
            get
            {
                return (CaptchaHttpHandlerTroubleshootingElement)this["captchaHttpHandlerTroubleshooting"];
            }
        }
    }
}
