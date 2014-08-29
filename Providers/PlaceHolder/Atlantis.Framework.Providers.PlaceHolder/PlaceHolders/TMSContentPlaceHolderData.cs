using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolders
{
  internal class TMSContentPlaceHolderData
  {
    private const string DATA_ELEMENT_NAME = "Data";
    private const string DEFAULT_ELEMENT_NAME = "default";
    private const string DEFAULT_ELEMENT_APP = "app";
    private const string DEFAULT_ELEMENT_LOCATION = "location";

    private IDictionary<string, string> _attributesDictionary;

    internal DefaultData Default { get; private set; }

    internal TMSContentPlaceHolderData(string dataXml)
    {
      Deserialize(dataXml);
    }

    internal TMSContentPlaceHolderData(IList<KeyValuePair<string, string>> attributes, DefaultData defaultData)
    {
      BuildAttributesDictionary(attributes);
      Default = defaultData;
    }

    public string Serialize()
    {
      string xml;

      try
      {
        XElement dataElement = new XElement(DATA_ELEMENT_NAME);

        // Write Attributes
        foreach (KeyValuePair<string, string> item in _attributesDictionary)
        {
          dataElement.Add(new XAttribute(item.Key, item.Value));
        }

        // Write Default Parameter
        if (Default != null)
        {
          XElement defaultElement = new XElement(DEFAULT_ELEMENT_NAME,
            new XAttribute(DEFAULT_ELEMENT_APP, Default.Application),
            new XAttribute(DEFAULT_ELEMENT_LOCATION, Default.Location));
          dataElement.Add(defaultElement);
        }

        xml = dataElement.ToString(SaveOptions.DisableFormatting);
      }
      catch (Exception ex)
      {
        xml = String.Empty;
        ErrorLogger.LogException(ex.Message, "TMSContentPlaceHolderData.Serialize()", ex.StackTrace);
      }

      return xml;
    }

    public override string ToString()
    {
      return Serialize();
    }

    internal bool TryGetAttribute(string name, out string value)
    {
      return _attributesDictionary.TryGetValue(name, out value);
    }

    private void BuildAttributesDictionary(IList<KeyValuePair<string, string>> attributes)
    {
      _attributesDictionary = new Dictionary<string, string>(attributes != null ? attributes.Count : 0);

      if (attributes != null)
      {
        foreach (KeyValuePair<string, string> keyValuePair in attributes)
        {
          if (keyValuePair.Key != null && keyValuePair.Value != null)
          {
            _attributesDictionary[keyValuePair.Key] = keyValuePair.Value;
          }
        }
      }
    }

    private void Deserialize(string dataXml)
    {
      try
      {
        XElement dataElement = XElement.Parse(dataXml, LoadOptions.None);

        // Read Attributes
        _attributesDictionary = new Dictionary<string, string>(16);
        foreach (XAttribute item in dataElement.Attributes())
        {
          _attributesDictionary[item.Name.ToString()] = item.Value;
        }

        // Read DefaultContent Parameter
        XElement defaultElement = dataElement.Element(DEFAULT_ELEMENT_NAME);
        if (defaultElement != null)
        {
          Default = new DefaultData();
          Default.Application = defaultElement.Attribute(DEFAULT_ELEMENT_APP).Value;
          Default.Location = defaultElement.Attribute(DEFAULT_ELEMENT_LOCATION).Value;
        }
        else
        {
          throw new ApplicationException(String.Format("TMSContent placeholder element, \"<default>\" must be defined. {0}", dataXml));
        }
      }
      catch (Exception ex)
      {
        _attributesDictionary = new Dictionary<string, string>(0);
        ErrorLogger.LogException(ex.Message, "TMSContentPlaceHolderData.Deserialize()", ex.StackTrace);
      }
    }

    public class DefaultData
    {
      public string Application { get; set; }

      public string Location { get; set; }
    }
  }
}
