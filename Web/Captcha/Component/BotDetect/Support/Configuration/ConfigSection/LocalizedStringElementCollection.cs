using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Globalization;

namespace BotDetect.Configuration
{
    // Define the "localizedString" element collection
    public class LocalizedStringElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new LocalizedStringElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LocalizedStringElement)element).Locale;
        }

        public void Add(LocalizedStringElement element)
        {
            BaseAdd(element);
        }

        protected override String ElementName
        {
            get { return "localizedString"; }
        }

        public Boolean ContainsKey(String key)
        {
            Boolean result = false;

            object[] keys = BaseGetAllKeys();
            foreach (object obj in keys)
            {
                if ((String)obj == key)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        internal new String this[String locale]
        {
            get 
            {
                // no configured values
                if (0 == this.Count)
                {
                    return null;
                }

                // multiple configured values, look for the best match
                String value = null;
                string[] localeParts = locale.ToLowerInvariant().Split('-');
                int maxMatchedParts = 0;
                int maxMatchedPartLength = 0;

                // compare all configured values to the current locale
                foreach (LocalizedStringElement elem in this)
                {
                    // configured locale
                    String configuredLocale = elem.Locale;

                    // universal definition has no "locale" value
                    if (!StringHelper.HasValue(configuredLocale))
                    {
                        value = elem.Value;
                        
                        // we don't increment matched part length,
                        // so any other match takes precedence 
                        // over the universal definition

                        continue;
                    }

                    // count matching declaration parts
                    string[] configuredParts = configuredLocale.ToLowerInvariant().Split('-');
                    int matchedParts = 0;
                    int matchedPartLength = 0;

                    foreach (string configuredPart in configuredParts)
                    {
                        foreach (string localePart in localeParts)
                        {
                            if (configuredPart == localePart)
                            {
                                matchedParts++;
                                matchedPartLength = configuredPart.Length;
                                break;
                            }
                        }
                    }

                    // more specific declrations are considered better matches
                    if (matchedParts > maxMatchedParts ||
                        (matchedParts == maxMatchedParts && matchedPartLength > maxMatchedPartLength))
                    {
                        value = elem.Value;
                        maxMatchedParts = matchedParts;
                        maxMatchedPartLength = matchedPartLength;
                    }
                }

                return value;
            }
        }
    }
}
