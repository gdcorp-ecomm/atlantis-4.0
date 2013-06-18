using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

namespace BotDetect.Configuration
{
    // Define the "characterSet" element
    public class CharacterSetElement : ConfigurationElement, ICharacterSetConfiguration
    {
        // Define the "name" attribute
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public String Name
        {
            get
            {
                return (String) this["name"];
            }
        }

        public StringCollection Alphanumeric
        {
            get
            {
                return StringHelper.ParseCsv(this.RawAlphanumeric);
            }
        }

        // Define the "alphanumeric" attribute
        [ConfigurationProperty("alphanumeric", IsRequired = true)]
        [RegexStringValidator(@"^(((\p{N}|\p{L}))(,(\p{N}|\p{L}))*)?$")]
        public String RawAlphanumeric
        {
            get
            {
                return (String)this["alphanumeric"];
            }
        }

        public StringCollection Alpha
        {
            get
            {
                return StringHelper.ParseCsv(this.RawAlpha);
            }
        }

        // Define the "alpha" attribute
        [ConfigurationProperty("alpha", IsRequired = false)]
        [RegexStringValidator(@"^((\p{L})(,\p{L})*)?$")]
        public String RawAlpha
        {
            get
            {
                return (String)this["alpha"];
            }
        }

        public StringCollection Numeric
        {
            get
            {
                return StringHelper.ParseCsv(this.RawNumeric);
            }
        }

        // Define the "numeric" attribute
        [ConfigurationProperty("numeric", IsRequired = false)]
        [RegexStringValidator(@"^((\p{N})(,\p{N})*)?$")]
        public String RawNumeric
        {
            get
            {
                return (String)this["numeric"];
            }
        }
    }    
}
