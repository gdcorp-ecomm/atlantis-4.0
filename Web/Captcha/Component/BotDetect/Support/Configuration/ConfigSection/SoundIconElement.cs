﻿using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "soundIcon" element
    public class SoundIconElement : ConfigurationElement
    {
        // Define the "soundIconTooltip" element collection
        [ConfigurationProperty("soundIconTooltip", IsRequired = false, IsDefaultCollection = false)]
        public LocalizedStringElementCollection Tooltip
        {
            get
            {
                return (LocalizedStringElementCollection)this["soundIconTooltip"];
            }
        }

        // Define the "filePath" attribute
        [ConfigurationProperty("filePath", IsRequired = false)]
        public String FilePath
        {
            get
            {
                return (String)this["filePath"];
            }
        }

        // Create a "iconWidth" element 
        [ConfigurationProperty("iconWidth", IsRequired = false)]
        public Int32 IconWidth
        {
            get
            {
                return (Int32)this["iconWidth"];
            }
        }
    }
}
