using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "reloadIcon" element
    public class ReloadIconElement : ConfigurationElement
    {
        // Create a "reloadIconTooltip" element collection.
        [ConfigurationProperty("reloadIconTooltip", IsRequired = false, IsDefaultCollection = false)]
        public LocalizedStringElementCollection Tooltip
        {
            get
            {
                return (LocalizedStringElementCollection)this["reloadIconTooltip"];
            }
        }

        // Create a "filePath" element 
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
