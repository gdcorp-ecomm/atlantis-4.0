using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolders
{
  internal class TMSContentPlaceHolderData
  {
    private const string DATA_ELEMENT_NAME = "Data";
    private const string DEFAULT_ELEMENT_NAME = "default";
    private const string DEFAULT_ELEMENT_APP = "app";
    private const string DEFAULT_ELEMENT_LOCATION = "location";

    private IDictionary<string, string> _attributesDictionary;

    internal TMSContentPlaceHolderData(string dataXml)
    {
      Deserialize(dataXml);
    }

    internal TMSContentPlaceHolderData(IList<KeyValuePair<string, string>> attributes, string defaultApplication, string defaultLocation)
    {
      BuildAttributesDictionary(attributes);
      DefaultApplication = defaultApplication;
      DefaultLocation = defaultLocation;
    }

    private string _appProduct;
    internal string AppProduct
    {
      get
      {
        if (_appProduct == null)
        {
          string appProduct;
          if (TryGetAttribute(PlaceHolderAttributes.AppProduct, out appProduct))
          {
            _appProduct = appProduct;
          }
          else
          {
            _appProduct = string.Empty;
          }
        }

        return _appProduct;
      }
    }

    private string _interactionName;
    internal string InteractionName
    {
      get
      {
        if (_interactionName == null)
        {
          string interactionName;
          if (TryGetAttribute(PlaceHolderAttributes.InteractionPoint, out interactionName))
          {
            _interactionName = interactionName;
          }
          else
          {
            _interactionName = string.Empty;
          }
        }

        return _interactionName;
      }
    }

    internal string DefaultApplication { get; private set; }

    internal string DefaultLocation { get; private set; }

    internal string Serialize()
    {
      string xml;

      try
      {
        XElement dataElement = new XElement(DATA_ELEMENT_NAME);

        foreach (KeyValuePair<string, string> item in _attributesDictionary)
        {
          dataElement.Add(new XAttribute(item.Key, item.Value));
        }

        if (DefaultApplication != null && DefaultLocation != null)
        {
          XElement defaultElement = new XElement(DEFAULT_ELEMENT_NAME,
                                      new XAttribute(DEFAULT_ELEMENT_APP, DefaultApplication),
                                      new XAttribute(DEFAULT_ELEMENT_LOCATION, DefaultLocation));
          
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

        _attributesDictionary = new Dictionary<string, string>(16);
        foreach (XAttribute item in dataElement.Attributes())
        {
          _attributesDictionary[item.Name.ToString()] = item.Value;
        }

        XElement defaultElement = dataElement.Element(DEFAULT_ELEMENT_NAME);
        if (defaultElement != null)
        {
          DefaultApplication = defaultElement.Attribute(DEFAULT_ELEMENT_APP).Value;
          DefaultLocation = defaultElement.Attribute(DEFAULT_ELEMENT_LOCATION).Value;
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
  }
}
