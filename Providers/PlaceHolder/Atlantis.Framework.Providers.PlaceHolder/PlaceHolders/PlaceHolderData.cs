using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  public class PlaceHolderData : IPlaceHolderData
  {
    private const string DATA_ELEMENT_NAME = "Data";
    private const string PARAMETERS_ELEMENT_NAME = "Parameters";
    private const string PARAMETER_ELEMENT_NAME = "Parameter";
    private const string KEY_ATTRIBUTE_NAME = "key";
    private const string VALUE_ATTRIBUTE_NAME = "value";

    private IDictionary<string, string> _attributesDictionary;
    private IDictionary<string, string> _parametersDictionary;

    internal PlaceHolderData()
    {
      _attributesDictionary = new Dictionary<string, string>(0);
      _parametersDictionary = new Dictionary<string, string>(0);
    }

    internal PlaceHolderData(IList<KeyValuePair<string, string>> attributes, IList<KeyValuePair<string, string>> parameters)
    {
      BuildAttributesDictionary(attributes);
      BuildParametersDictionary(parameters);
    }

    internal PlaceHolderData(string dataXml)
    {
      Deserialize(dataXml);
    } 

    private void BuildAttributesDictionary(IList<KeyValuePair<string, string>> attributes)
    {
      BuildDictionary(attributes, out _attributesDictionary);
    }

    private void BuildParametersDictionary(IList<KeyValuePair<string, string>> parameters)
    {
      BuildDictionary(parameters, out _parametersDictionary);
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
          _parametersDictionary = new Dictionary<string, string>(0);
        }
        else
        {
          _parametersDictionary = new Dictionary<string, string>(32);
          IEnumerable<XElement> parameterElements = parametersElement.Elements(PARAMETER_ELEMENT_NAME);

          foreach (XElement parameterElement in parameterElements)
          {
            XAttribute keyAttribute = parameterElement.Attribute(KEY_ATTRIBUTE_NAME);
            XAttribute valueAttribute = parameterElement.Attribute(VALUE_ATTRIBUTE_NAME);
            
            if (keyAttribute != null && valueAttribute != null)
            {
              _parametersDictionary[keyAttribute.Value] = valueAttribute.Value;
            }
          }
        }
      }
      catch(Exception ex)
      {
        _attributesDictionary = new Dictionary<string, string>(0);
        _parametersDictionary = new Dictionary<string, string>(0);
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

        if (_parametersDictionary.Count > 0)
        {
          XElement parametersElement = new XElement(PARAMETERS_ELEMENT_NAME);

          foreach (KeyValuePair<string, string> parameterPair in _parametersDictionary)
          {
            XElement parameterElement = new XElement(PARAMETER_ELEMENT_NAME);
            parameterElement.Add(new XAttribute(KEY_ATTRIBUTE_NAME, parameterPair.Key));
            parameterElement.Add(new XAttribute(VALUE_ATTRIBUTE_NAME, parameterPair.Value));

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

    public bool TryGetParameter(string key, out string value)
    {
      return _parametersDictionary.TryGetValue(key, out value);
    }

    public override string ToString()
    {
      return Serialize();
    }
  }
}
