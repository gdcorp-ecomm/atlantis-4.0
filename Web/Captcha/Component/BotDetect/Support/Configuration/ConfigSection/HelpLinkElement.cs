using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.ComponentModel;
using System.Globalization;

namespace BotDetect.Configuration
{
    // Define the "helpLink" element
    public class HelpLinkElement : ConfigurationElement
    {
        // Define the "enabled" attribute
        [ConfigurationProperty("enabled", DefaultValue = CaptchaDefaults.HelpLinkEnabled, IsRequired = false)]
        public Boolean Enabled
        {
            get
            {
                return (Boolean)this["enabled"];
            }
        }

        // define the "mode" attribute


        [ConfigurationProperty("mode", DefaultValue = HelpLinkMode.Image, IsRequired = false)]

        [TypeConverter(typeof(CaseInsensitiveEnumConfigConverter<HelpLinkMode>))]
        public HelpLinkMode Mode { get { return (HelpLinkMode)this["mode"]; } }


        // Define the "helpPage" element collection
        [ConfigurationProperty("helpPage", IsRequired = false, IsDefaultCollection = false)]
        public LocalizedStringElementCollection HelpPage
        {
            get
            {
                return (LocalizedStringElementCollection)this["helpPage"];
            }
        }

        // Define the "helpText" element collection
        [ConfigurationProperty("helpText", IsRequired = false, IsDefaultCollection = false)]
        public LocalizedStringElementCollection HelpText
        {
            get
            {
                return (LocalizedStringElementCollection)this["helpText"];
            }
        }
    }


    /// <summary>
    /// helper for case-insensitive enumerated config values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CaseInsensitiveEnumConfigConverter<T> : ConfigurationConverterBase
    {
        public override object ConvertFrom(
        ITypeDescriptorContext context, CultureInfo ci, object data)
        {
            return Enum.Parse(typeof(T), (string)data, true);
        }
    }

}
