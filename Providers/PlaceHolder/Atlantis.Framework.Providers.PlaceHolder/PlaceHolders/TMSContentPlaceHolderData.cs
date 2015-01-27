using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolders
{
  internal class TMSContentPlaceHolderData
  {
    public const string XML_ELEMNAME_DATA = "data";
    private const string VALID_DEFAULT_ATTRIBUTE_VALUE_EXP = @"[_a-zA-Z0-9\-]";

    private readonly IDictionary<string, string> _attributesDictionary =
      new Dictionary<string, string>(5, StringComparer.OrdinalIgnoreCase);

    public string Product
    {
      get
      {
        string value;
        if (TryGetAttribute(PlaceHolderAttributes.TMS_Product, out value))
        {
          Match match = Regex.Match(value, VALID_DEFAULT_ATTRIBUTE_VALUE_EXP);
          if (!match.Success)
          {
            throw new ArgumentException(string.Format("TMSContent placeholder contains invalid value for required attribute '{0}'.",
              Enum.GetName(typeof(PlaceHolderAttributes), PlaceHolderAttributes.TMS_Product)));
          }
          return value;
        }

        // Required Attribute
        throw new ArgumentException(string.Format("TMSContent placeholder definition is missing required attribute '{0}'.",
          Enum.GetName(typeof (PlaceHolderAttributes), PlaceHolderAttributes.TMS_Product)));
      }
    }

    public string Interaction
    {
      get
      {
        string value;
        if (TryGetAttribute(PlaceHolderAttributes.TMS_Interaction, out value))
        {
          Match match = Regex.Match(value, VALID_DEFAULT_ATTRIBUTE_VALUE_EXP);
          if (!match.Success)
          {
            throw new ArgumentException(string.Format("TMSContent placeholder contains invalid value for required attribute '{0}'.",
              Enum.GetName(typeof(PlaceHolderAttributes), PlaceHolderAttributes.TMS_Interaction)));
          }
          return value;
        }

        // Required Attribute
        throw new ArgumentException(string.Format("TMSContent placeholder definition is missing required attribute '{0}'.",
          Enum.GetName(typeof (PlaceHolderAttributes), PlaceHolderAttributes.TMS_Interaction)));
      }
    }

    public string Channel
    {
      get
      {
        string value;
        if (TryGetAttribute(PlaceHolderAttributes.TMS_Channel, out value))
        {
          Match match = Regex.Match(value, VALID_DEFAULT_ATTRIBUTE_VALUE_EXP);
          if (!match.Success)
          {
            throw new ArgumentException(string.Format("TMSContent placeholder contains invalid value for required attribute '{0}'.",
              Enum.GetName(typeof(PlaceHolderAttributes), PlaceHolderAttributes.TMS_Channel)));
          }
          return value;
        }

        // Required Attribute
        throw new ArgumentException(string.Format("TMSContent placeholder definition is missing required attribute '{0}'.",
          Enum.GetName(typeof(PlaceHolderAttributes), PlaceHolderAttributes.TMS_Channel)));
      }
    }

    public string Template
    {
      get
      {
        string value;
        if (TryGetAttribute(PlaceHolderAttributes.TMS_Template, out value))
        {
          Match match = Regex.Match(value, VALID_DEFAULT_ATTRIBUTE_VALUE_EXP);
          if (!match.Success)
          {
            throw new ArgumentException(string.Format("TMSContent placeholder contains invalid value for required attribute '{0}'.",
              Enum.GetName(typeof(PlaceHolderAttributes), PlaceHolderAttributes.TMS_Template)));
          }
          return value;
        }

        // Required Attribute
        throw new ArgumentException(string.Format("TMSContent placeholder definition is missing required attribute '{0}'.",
          Enum.GetName(typeof(PlaceHolderAttributes), PlaceHolderAttributes.TMS_Template)));
      }
    }

    public int? Rank
    {
      get
      {
        int iValue;
        string value;
        if (TryGetAttribute(PlaceHolderAttributes.TMS_Rank, out value) && int.TryParse(value, out iValue))
        {
          return iValue;
        }

        // Optional Attribute
        return null;
      }
    }

    public TMSContentPlaceHolderData(string dataXml)
    {
      Deserialize(this, dataXml);
    }

    internal TMSContentPlaceHolderData(IEnumerable<KeyValuePair<string, string>> attributes)
    {
      // Initialize Attribute(s)
      if (attributes != null)
      {
        foreach (KeyValuePair<string, string> attribute in attributes)
        {
          if ((attribute.Key != null) && !string.IsNullOrEmpty(attribute.Value))
          {
            _attributesDictionary[attribute.Key] = attribute.Value;
          }
        }
      }
    }

    private void Deserialize(TMSContentPlaceHolderData obj, string dataXml)
    {
      XElement xElementData = XElement.Parse(dataXml, LoadOptions.None);

      // Deserialize Attribute(s)
      foreach (XAttribute item in xElementData.Attributes())
      {
        if (!string.IsNullOrEmpty(item.Value))
        {
          obj._attributesDictionary[item.Name.ToString()] = item.Value;
        }
      }
    }

    internal XElement Serialize()
    {
      XElement xElementData = new XElement(XML_ELEMNAME_DATA);

      // Serialize Attribute(s)
      foreach (KeyValuePair<string, string> item in _attributesDictionary)
      {
        if (!string.IsNullOrEmpty(item.Value))
        {
          xElementData.Add(new XAttribute(item.Key, item.Value));
        }
      }

      return xElementData;
    }

    public override string ToString()
    {
      return Serialize().ToString(SaveOptions.DisableFormatting);
    }

    private bool TryGetAttribute(string name, out string value)
    {
      return _attributesDictionary.TryGetValue(name, out value) && !string.IsNullOrEmpty(value);
    }
  }
}
