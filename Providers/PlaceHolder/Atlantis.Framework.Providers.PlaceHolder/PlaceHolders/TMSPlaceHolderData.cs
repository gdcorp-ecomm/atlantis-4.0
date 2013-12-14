using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolders
{
  internal class TMSPlaceHolderData
  {
    private const string DATA_ELEMENT_NAME = "Data";
    private const string PARAMETERS_ELEMENT_NAME = "MessageTags";
    private const string PARAMETER_ELEMENT_NAME = "MessageTag";
  
    private IDictionary<string, string> _attributesDictionary;
    
    private IList<string> _messageTags;
    internal IList<string> MessageTags
    {
      get { return _messageTags; }
    }

    internal TMSPlaceHolderData(IList<KeyValuePair<string, string>> attributes, IList<string> parameters)
    {
      BuildAttributesDictionary(attributes);
      _messageTags = parameters;
    }

    internal TMSPlaceHolderData(string dataXml)
    {
      Deserialize(dataXml);
    } 

    private void BuildAttributesDictionary(IList<KeyValuePair<string, string>> attributes)
    {
      BuildDictionary(attributes, out _attributesDictionary);
    }

    private void BuildDictionary(IList<KeyValuePair<string, string>> keyValuePairs, out IDictionary<string, string> dictionary)
    {
      dictionary = new Dictionary<string, string>(keyValuePairs != null ? keyValuePairs.Count : 0);

      if (keyValuePairs != null)
      {
        foreach (KeyValuePair<string, string> keyValuePair in keyValuePairs)
        {
          if (keyValuePair.Key != null && keyValuePair.Value != null)
          {
            dictionary[keyValuePair.Key] = keyValuePair.Value;
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

        foreach (XAttribute dataAttribute in dataElement.Attributes())
        {
          _attributesDictionary[dataAttribute.Name.ToString()] = dataAttribute.Value;
        }

        XElement parametersElement = dataElement.Element(PARAMETERS_ELEMENT_NAME);
        if (parametersElement == null)
        {
          _messageTags = new List<string>(0);
        }
        else
        {
          _messageTags = new List<string>(32);
          IEnumerable<XElement> parameterElements = parametersElement.Elements(PARAMETER_ELEMENT_NAME);

          foreach (XElement parameterElement in parameterElements)
          {
            string value = parameterElement.Value;
            
            if (!string.IsNullOrEmpty(value))
            {
              _messageTags.Add(value);
            }
          }
        }
      }
      catch(Exception ex)
      {
        _attributesDictionary = new Dictionary<string, string>(0);
        _messageTags = new List<string>(0);
        ErrorLogger.LogException(ex.Message, "PlaceHolderData.Deserialize()", ex.StackTrace);
      }
    }

    public string Serialize()
    {
      string xml;

      try
      {
        XElement dataElement = new XElement(DATA_ELEMENT_NAME);

        foreach (KeyValuePair<string, string> attributePair in _attributesDictionary)
        {
          dataElement.Add(new XAttribute(attributePair.Key, attributePair.Value));
        }

        if (_messageTags.Count > 0)
        {
          XElement parametersElement = new XElement(PARAMETERS_ELEMENT_NAME);

          foreach (string value in _messageTags)
          {
            XElement parameterElement = new XElement(PARAMETER_ELEMENT_NAME);
            parameterElement.Value = value;
            parametersElement.Add(parameterElement);
          }

          dataElement.Add(parametersElement);
        }

        xml = dataElement.ToString(SaveOptions.DisableFormatting);
      }
      catch (Exception ex)
      {
        xml = string.Empty;
        ErrorLogger.LogException(ex.Message, "PlaceHolderData.Serialize()", ex.StackTrace);
      }

      return xml;
    }

    internal bool TryGetAttribute(string name, out string value)
    {
      return _attributesDictionary.TryGetValue(name, out value);
    }

    public override string ToString()
    {
      return Serialize();
    }

  }
}
