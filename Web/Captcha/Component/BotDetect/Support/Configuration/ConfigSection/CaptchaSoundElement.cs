using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "captchaSound" element
    public class CaptchaSoundElement : ConfigurationElement
    {
        // Define the "enabled" attribute
        [ConfigurationProperty("enabled", DefaultValue = CaptchaDefaults.SoundEnabled, IsRequired = false)]
        public Boolean Enabled
        {
            get
            {
                return (Boolean)this["enabled"];
            }
        }

        // Define the "enabled" attribute
        [ConfigurationProperty("startDelay", DefaultValue = CaptchaDefaults.SoundStartDelay, IsRequired = false)]
        public Int32 StartDelay
        {
            get
            {
                return (Int32)this["startDelay"];
            }
        }

        // Define the "soundIcon" sub-element
        [ConfigurationProperty("soundIcon", IsRequired = false)]
        public SoundIconElement SoundIcon
        {
            get
            {
                return (SoundIconElement)this["soundIcon"];
            }
        }

        // Define the "disabledSoundStyles" element collection
        [ConfigurationProperty("disabledSoundStyles", IsRequired = false, IsDefaultCollection = false)]
        public DisabledSoundStylesElement DisabledSoundStyles
        {
            get
            {
                return (DisabledSoundStylesElement)this["disabledSoundStyles"];
            }
        }

        // Define the "soundPackages" sub-element
        [ConfigurationProperty("soundPackages", IsRequired = false)]
        public SoundPackagesElement SoundPackages
        {
            get
            {
                return (SoundPackagesElement)this["soundPackages"];
            }
        }
    }
}
