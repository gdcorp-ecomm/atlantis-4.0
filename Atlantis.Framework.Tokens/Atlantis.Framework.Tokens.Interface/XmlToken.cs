using System;
using System.Xml.Linq;

namespace Atlantis.Framework.Tokens.Interface
{
  public class XmlToken : TokenBase<XElement>
  {
    public XmlToken(string key, string data, string fullTokenString)
      : base(key, data, fullTokenString)
    {
    }

    protected override XElement DeserializeTokenData(string data)
    {
      XElement result;

      if (string.IsNullOrEmpty(data))
      {
        result = new XElement("empty");
      }
      else
      {
        result = XElement.Parse(data);
      }

      return result;
    }

    protected string GetAttributeText(string attributeName, string defaultValue, XElement element = null)
    {
      if (element == null)
      {
        element = TokenData;
      }

      string result = defaultValue;

      if (element != null)
      {
        XAttribute attribute = element.Attribute(attributeName);
        if (attribute != null)
        {
          result = attribute.Value;
        }
      }

      return result;
    }

    protected int GetAttributeInt(string attributeName, int defaultValue, XElement element = null)
    {
      if (element == null)
      {
        element = TokenData;
      }

      int result = defaultValue;

      if (element != null)
      {
        XAttribute attribute = element.Attribute(attributeName);
        if (attribute != null)
        {
          int valueInt;
          if (int.TryParse(attribute.Value, out valueInt))
          {
            result = valueInt;
          }
        }
      }

      return result;
    }

    protected double GetAttributeDouble(string attributeName, double defaultValue, XElement element = null)
    {
      if (element == null)
      {
        element = TokenData;
      }

      double result = defaultValue;

      if (element != null)
      {
        XAttribute attribute = element.Attribute(attributeName);
        if (attribute != null)
        {
          double valueDouble;
          if (double.TryParse(attribute.Value, out valueDouble))
          {
            result = valueDouble;
          }
        }
      }

      return result;
    }

    protected bool GetAttributeBool(string attributeName, bool defaultValue, XElement element = null)
    {
      if (element == null)
      {
        element = TokenData;
      }

      bool result = defaultValue;

      if (element != null)
      {
        XAttribute attribute = element.Attribute(attributeName);
        if (attribute != null)
        {
          result = (("true".Equals(attribute.Value, StringComparison.OrdinalIgnoreCase)) || ("1".Equals(attribute.Value, StringComparison.Ordinal)));
        }
      }

      return result;
    }

    protected DateTime GetAttributeDateTime(string attributeName, DateTime defaultValue, XElement element = null)
    {
      if (element == null)
      {
        element = TokenData;
      }

      DateTime result = defaultValue;

      if (element != null)
      {
        XAttribute attribute = element.Attribute(attributeName);
        if (attribute != null)
        {
          DateTime valueDate;
          if (DateTime.TryParse(attribute.Value, out valueDate))
          {
            result = valueDate;
          }
        }
      }

      return result;
    }

  }
}
