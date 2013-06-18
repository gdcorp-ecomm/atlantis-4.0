using System;
using System.Collections;
using System.Text;
using System.Configuration;

namespace BotDetect.Configuration
{
    // Define the "characterSets" element collection
    public class CharacterSetElementCollection : ConfigurationElementCollection
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
            return new CharacterSetElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CharacterSetElement)element).Name;
        }

        public void Add(CharacterSetElement element)
        {
            BaseAdd(element);
        }

        protected override String ElementName
        {
            get { return "characterSet"; }
        }

        new public CharacterSetElement this[String name]
        {
            get
            {
                return (CharacterSetElement)BaseGet(name);
            }
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
    }
}
