using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "captchaImage" element
    public class CaptchaImageElement : ConfigurationElement
    {
        // Define the "captchaImageTooltip" sub-element collection
        [ConfigurationProperty("captchaImageTooltip", IsRequired = false, IsDefaultCollection = false)]
        public LocalizedStringElementCollection Tooltip
        {
            get
            {
                return (LocalizedStringElementCollection)this["captchaImageTooltip"];
            }
        }

        // Define the "disabledImageStyles" element collection
        [ConfigurationProperty("disabledImageStyles", IsRequired = false, IsDefaultCollection = false)]
        public DisabledImageStylesElement DisabledImageStyles
        {
            get
            {
                return (DisabledImageStylesElement)this["disabledImageStyles"];
            }
        }

        // Define the "helpLink" element collection
        [ConfigurationProperty("helpLink", IsRequired = false, IsDefaultCollection = false)]
        public HelpLinkElement HelpLink
        {
            get
            {
                return (HelpLinkElement)this["helpLink"];
            }
        }
    }
}
