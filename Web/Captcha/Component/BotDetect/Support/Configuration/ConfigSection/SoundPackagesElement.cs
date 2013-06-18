using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "soundPackages" element
    public class SoundPackagesElement : ConfigurationElement
    {
        // Create a "folderPath" element.
        [ConfigurationProperty("folderPath", DefaultValue = CaptchaDefaults.SoundPackagesFolder, IsRequired = false)]
        public String FolderPath
        {
            get
            {
                return (String)this["folderPath"];
            }
        }

        // Define the "warnAboutMissingSoundPackages" attribute
        [ConfigurationProperty("warnAboutMissingSoundPackages", DefaultValue = CaptchaDefaults.WarnAboutMissingSoundPackages, IsRequired = false)]
        public Boolean WarnAboutMissingSoundPackages
        {
            get
            {
                return (Boolean)this["warnAboutMissingSoundPackages"];
            }
        }
    }
}
