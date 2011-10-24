using System.Collections.Generic;
using System.Xml;

namespace Atlantis.Framework.GetBasketPrice.Interface
{
	public class BasketPriceItem
	{
    // **************************************************************** //

    Dictionary<string, string> m_dictItemAttrs = new Dictionary<string, string>();
    string m_sCustomXML = "";

    // **************************************************************** //

    public BasketPriceItem(XmlElement xlItem)
    {
      foreach (XmlAttribute attr in xlItem.Attributes)
        m_dictItemAttrs.Add(attr.Name, attr.Value);

      XmlNode xnCustomXML = xlItem.SelectSingleNode("./CUSTOMXML");
      if (xnCustomXML != null)
        m_sCustomXML = xnCustomXML.InnerXml;
    }

    // **************************************************************** //

    public Dictionary<string, string> Attributes
    {
      get { return m_dictItemAttrs; }
    }

    // **************************************************************** //

    public string GetAttribute(string sName)
    {
      string sValue = "";
      if (m_dictItemAttrs.ContainsKey(sName))
        sValue = m_dictItemAttrs[sName];
      return sValue;
    }

    // **************************************************************** //

    public string CustomXML
    {
      get { return m_sCustomXML; }
    }

    // **************************************************************** //
  }
}
