using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetBasketPrice.Interface
{
  public class GetBasketPriceResponseData : IResponseData
  {
    // **************************************************************** //

    bool m_bXmlParsed = false;
    string m_sResponseXML;
    AtlantisException m_ex;
    List<BasketPriceItem> m_lstBasketPriceItems = new List<BasketPriceItem>();
    Dictionary<string, string> m_dictOrderDetail = new Dictionary<string, string>();
    string m_sDomainContactXML = string.Empty;

    // **************************************************************** //

    public GetBasketPriceResponseData(string sBasketXML)
    {
      m_sResponseXML = sBasketXML;
      m_ex = null;
    }

    // **************************************************************** //

    public GetBasketPriceResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      m_sResponseXML = sResponseXML;
      m_ex = exAtlantis;
    }

    // **************************************************************** //

    public GetBasketPriceResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      m_sResponseXML = sResponseXML;
      m_ex = new AtlantisException(oRequestData,
                                   "GetBasketPriceResponseData", 
                                   ex.Message, 
                                   oRequestData.ToXML());
    }
    
    // **************************************************************** //

    public string GetItemAttribute(int index, string sName)
    {
      PopulateFromXML(m_sResponseXML);
      return m_lstBasketPriceItems[index].GetAttribute(sName);
    }

    // **************************************************************** //

    public Dictionary<string, string> GetItemAttributes(int index)
    {
      PopulateFromXML(m_sResponseXML);
      return m_lstBasketPriceItems[index].Attributes;
    }

    // **************************************************************** //

    public string GetItemCustomXML(int index)
    {
      PopulateFromXML(m_sResponseXML);
      return m_lstBasketPriceItems[index].CustomXML;
    }

    // **************************************************************** //

    public string GetOrderDetailAttribute(string sName)
    {
      PopulateFromXML(m_sResponseXML);
      string sValue = string.Empty;
      if (m_dictOrderDetail.ContainsKey(sName))
        sValue = m_dictOrderDetail[sName];
      return sValue;
    }

    // **************************************************************** //

    public int ItemCount
    {
      get
      {
        PopulateFromXML(m_sResponseXML);
        return m_lstBasketPriceItems.Count;
      }
    }

    // **************************************************************** //

    public string DomainContactXML
    {
      get { return m_sDomainContactXML; }
    }

    // **************************************************************** //

    void PopulateFromXML(string sBasketXML)
    {
      if (m_bXmlParsed)
        return;
      XmlDocument xdDoc = new XmlDocument();

      xdDoc.LoadXml(sBasketXML);

      XmlNode xnOrderDetail = xdDoc.SelectSingleNode("/ORDER/ORDERDETAIL");
      if (xnOrderDetail != null)
      {
        foreach (XmlAttribute attr in xnOrderDetail.Attributes)
          m_dictOrderDetail.Add(attr.Name, attr.Value);
      }

      XmlNode xnDomainContactXML = xdDoc.SelectSingleNode("/ORDER/ORDERDETAIL/DOMAINCONTACTXML");
      if (xnDomainContactXML != null)
        m_sDomainContactXML = xnDomainContactXML.InnerXml;

      XmlNodeList xnlBasketPriceItems = xdDoc.SelectNodes("/ORDER/ITEMS/ITEM");
      foreach (XmlElement xlBasketPriceItem in xnlBasketPriceItems)
        m_lstBasketPriceItems.Add(new BasketPriceItem(xlBasketPriceItem));

      m_bXmlParsed = true;
    }

    public bool IsSuccess
    {
      get { return m_sResponseXML.IndexOf("success", StringComparison.OrdinalIgnoreCase) > -1; }
    }

    // **************************************************************** //

    #region IResponseData Members

    // **************************************************************** //

    public string ToXML()
    {
      return m_sResponseXML;
    }

    // **************************************************************** //

    public AtlantisException GetException()
    {
      return m_ex;
    }

    // **************************************************************** //

    #endregion

    // **************************************************************** //
  }
}
