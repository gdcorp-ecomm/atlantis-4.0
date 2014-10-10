using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolders
{
  internal class TMSContentPlaceHolderData
  {
    private const string CONTENT_ELEMENT_NAME = "content";
    private const string DATA_ELEMENT_NAME = "Data";
    private const string DEFAULT_ELEMENT_NAME = "default";

    private string _appProduct;
    private IDictionary<string, string> _attributesDictionary;

    private string _interactionName;

    internal string AppProduct
    {
      get
      {
        if (_appProduct == null)
        {
          string appProduct;
          _appProduct = TryGetAttribute(PlaceHolderAttributes.AppProduct, out appProduct)
            ? appProduct : string.Empty;
        }

        return _appProduct;
      }
    }

    internal string InteractionName
    {
      get
      {
        if (_interactionName == null)
        {
          string interactionName;
          _interactionName = TryGetAttribute(PlaceHolderAttributes.InteractionPoint, out interactionName)
            ? interactionName : string.Empty;
        }

        return _interactionName;
      }
    }

    internal ContentElementData ContentElement { get; private set; }

    internal DefaultElementData DefaultElement { get; private set; }

    internal TMSContentPlaceHolderData(string dataXml)
    {
      Deserialize(dataXml);
    }

    internal TMSContentPlaceHolderData(IList<KeyValuePair<string, string>> attributes,
      DefaultElementData defaultElement, ContentElementData contentElement = null)
    {
      BuildAttributesDictionary(attributes);

      // DefaultElement (Required)
      if ((defaultElement == null) || (!defaultElement.IsValid()))
      {
        throw new ApplicationException(String.Format("TMSContent placeholder element, \"<default>\" must be defined."));
      }

      // ContentElement (Optional)
      ContentElement = ((contentElement == null) || (!contentElement.IsValid())) ? contentElement : null;
    }

    public override string ToString()
    {
      return Serialize();
    }

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

        // DefaultElement (Required)
        XElement defaultElement = new XElement(DEFAULT_ELEMENT_NAME,
          new XAttribute(PlaceHolderAttributes.Application, DefaultElement.App),
          new XAttribute(PlaceHolderAttributes.Location, DefaultElement.Location));
        dataElement.Add(defaultElement);

        // ContentElement (Optional)
        if (ContentElement != null)
        {
          XElement contentElement = new XElement(DEFAULT_ELEMENT_NAME,
            new XAttribute(PlaceHolderAttributes.Application, ContentElement.App),
            new XAttribute(PlaceHolderAttributes.Location, ContentElement.Location));

          dataElement.Add(contentElement);
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

        // DefaultElement (Required)
        XElement defaultElement = dataElement.Element(DEFAULT_ELEMENT_NAME);
        if (defaultElement != null)
        {
          DefaultElement = new DefaultElementData(
            defaultElement.Attribute(PlaceHolderAttributes.Application).Value,
            defaultElement.Attribute(PlaceHolderAttributes.Location).Value);
        }
        else
        {
          throw new ApplicationException(String.Format("TMSContent placeholder element, \"<default>\" must be defined. {0}", dataXml));
        }

        // ContentElement (Optional)
        XElement contentElement = dataElement.Element(CONTENT_ELEMENT_NAME);
        if (contentElement != null)
        {
          ContentElement = new ContentElementData(
            contentElement.Attribute(PlaceHolderAttributes.Application).Value,
            contentElement.Attribute(PlaceHolderAttributes.Location).Value);
        }
      }
      catch (Exception ex)
      {
        _attributesDictionary = new Dictionary<string, string>(0);
        ErrorLogger.LogException(ex.Message, "TMSContentPlaceHolderData.Deserialize()", ex.StackTrace);
      }
    }

    #region Nested type: ContentElementData

    public class ContentElementData
    {
      public string App { get; set; }
      public string Location { get; set; }
      public bool OverrideDocumentName { get; set; }

      public ContentElementData(string app, string location, bool overrideDocumentName = false)
      {
        App = app;
        Location = location;
        OverrideDocumentName = overrideDocumentName;
      }

      public bool IsValid()
      {
        return (!string.IsNullOrEmpty(App) && !string.IsNullOrEmpty(Location));
      }
    }

    #endregion

    #region Nested type: DefaultElementData

    public class DefaultElementData
    {
      public string App { get; set; }
      public string Location { get; set; }

      public DefaultElementData(string app, string location)
      {
        App = app;
        Location = location;
      }

      public bool IsValid()
      {
        return (!string.IsNullOrEmpty(App) && !string.IsNullOrEmpty(Location));
      }
    }

    #endregion
  }
}
