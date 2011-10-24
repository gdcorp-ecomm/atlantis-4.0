using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetBasket.Interface
{
  public class GetBasketResponseData : IResponseData
  {
    string _ResponseXML;
    AtlantisException _exception;
    List<BasketItem> _basketItems = new List<BasketItem>();
    Dictionary<string, string> _OrderDetail = new Dictionary<string, string>();
    string domainContactXml = "";

    public GetBasketResponseData(string sBasketXML)
    {
      _ResponseXML = sBasketXML;
      _exception = null;
      PopulateFromXML(sBasketXML);
    }

    public GetBasketResponseData(string sResponseXML, AtlantisException exAtlantis)
    {
      _ResponseXML = sResponseXML;
      _exception = exAtlantis;
    }

    public GetBasketResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      _ResponseXML = sResponseXML;
      _exception = new AtlantisException(oRequestData, 
                                   "GetBasketResponseData", 
                                   ex.Message, 
                                   oRequestData.ToXML());
    }
    
    public string GetItemAttribute(int index, string sName)
    {
      return _basketItems[index].GetAttribute(sName);
    }

    public Dictionary<string, string> GetItemAttributes(int index)
    {
      return _basketItems[index].Attributes;
    }

    public string GetItemCustomXML(int index)
    {
      return _basketItems[index].CustomXML;
    }

    public string GetOrderDetailAttribute(string sName)
    {
      string sValue = "";
      if (_OrderDetail.ContainsKey(sName))
        sValue = _OrderDetail[sName];
      return sValue;
    }

    public int ItemCount
    {
      get { return _basketItems.Count; }
    }

    public string DomainContactXML
    {
      get { return domainContactXml; }
    }

    void PopulateFromXML(string sBasketXML)
    {
      XmlDocument xdDoc = new XmlDocument();

      xdDoc.LoadXml(sBasketXML);

      XmlNode xnOrderDetail = xdDoc.SelectSingleNode("/ORDER/ORDERDETAIL");
      if (xnOrderDetail != null)
      {
        foreach (XmlAttribute attr in xnOrderDetail.Attributes)
          _OrderDetail.Add(attr.Name, attr.Value);
      }

      XmlNode xnDomainContactXML = xdDoc.SelectSingleNode("/ORDER/ORDERDETAIL/DOMAINCONTACTXML");
      if (xnDomainContactXML != null)
        domainContactXml = xnDomainContactXML.InnerXml;

      XmlNodeList xnlBasketItems = xdDoc.SelectNodes("/ORDER/ITEMS/ITEM");
      foreach (XmlElement xlBasketItem in xnlBasketItems)
        _basketItems.Add(new BasketItem(xlBasketItem));
    }

    public bool IsSuccess
    {
      get { return (_exception == null); }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _ResponseXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    
    #endregion

  }
}
