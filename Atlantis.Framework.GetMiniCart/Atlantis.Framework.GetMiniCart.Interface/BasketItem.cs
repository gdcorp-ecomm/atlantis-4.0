using System.Collections.Generic;
using System.Xml;

namespace Atlantis.Framework.GetMiniCart.Interface
{
  class BasketItem
  {
    private readonly Dictionary<string, string> _itemAttributes = new Dictionary<string, string>();
    private readonly string _customXml = string.Empty;

    public BasketItem(XmlElement itemElement)
    {
      foreach (XmlAttribute attr in itemElement.Attributes)
      {
        _itemAttributes.Add(attr.Name, attr.Value);
      }

      XmlNode xnCustomXML = itemElement.SelectSingleNode("./CUSTOMXML");
      if (xnCustomXML != null)
      {
        _customXml = xnCustomXML.InnerXml;
      }
    }

    public Dictionary<string, string> Attributes
    {
      get { return _itemAttributes; }
    }

    public string GetAttribute(string name)
    {
      string value = string.Empty;
      if (_itemAttributes.ContainsKey(name))
      {
        value = _itemAttributes[name];
      }
      return value;
    }

    public string CustomXML
    {
      get { return _customXml; }
    }
  }
}
