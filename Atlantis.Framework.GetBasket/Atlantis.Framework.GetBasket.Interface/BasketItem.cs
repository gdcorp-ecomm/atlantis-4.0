using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.GetBasket.Interface
{
  class BasketItem
  {
    
    Dictionary<string, string> _dictItemAttrs = new Dictionary<string, string>();
    string customXML = "";

    public BasketItem(XmlElement xmlElement)
    {
      foreach (XmlAttribute attr in xmlElement.Attributes)
        _dictItemAttrs.Add(attr.Name, attr.Value);

      XmlNode xmlCustom = xmlElement.SelectSingleNode("./CUSTOMXML");
      if (xmlCustom != null)
        customXML = xmlCustom.InnerXml;
    }

    public Dictionary<string, string> Attributes
    {
      get { return _dictItemAttrs; }
    }

    public string GetAttribute(string name)
    {
      string value = "";
      if (_dictItemAttrs.ContainsKey(name))
        value = _dictItemAttrs[name];
      return value;
    }

    public string CustomXML
    {
      get { return customXML; }
    }

  }
}
