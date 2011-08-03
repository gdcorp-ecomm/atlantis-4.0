using System;
using System.Collections.Generic;
using System.Xml;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetMiniCart.Interface
{
  public class GetMiniCartResponseData : IResponseData
  {
    private readonly List<BasketItem> _basketItems = new List<BasketItem>();
    private readonly Dictionary<string, string> _orderDetail = new Dictionary<string, string>();
    private string _domainContactXml = string.Empty;
    private readonly string _responseXml;
    private readonly AtlantisException _exception;

    public GetMiniCartResponseData(string miniCartXml)
    {
      _responseXml = miniCartXml;
      _exception = null;
      PopulateFromXML(miniCartXml);
    }

    public GetMiniCartResponseData(string responseXML, AtlantisException exAtlantis)
    {
      _responseXml = responseXML;
      _exception = exAtlantis;
    }

    public GetMiniCartResponseData(string responseXML, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXML;
      _exception = new AtlantisException(oRequestData,
                                   "GetMiniCartResponseData", 
                                   ex.Message, 
                                   oRequestData.ToXML());
    }

    public string GetItemAttribute(int index, string name)
    {
      return _basketItems[index].GetAttribute(name);
    }

    public Dictionary<string, string> GetItemAttributes(int index)
    {
      return _basketItems[index].Attributes;
    }

    public string GetItemCustomXML(int index)
    {
      return _basketItems[index].CustomXML;
    }

    public string GetOrderDetailAttribute(string name)
    {
      string value = string.Empty;
      if (_orderDetail.ContainsKey(name))
      {
        value = _orderDetail[name];
      }
      return value;
    }

    public int ItemCount
    {
      get { return _basketItems.Count; }
    }

    public string DomainContactXML
    {
      get { return _domainContactXml; }
    }

    void PopulateFromXML(string basketXml)
    {
      XmlDocument xdDoc = new XmlDocument();
      xdDoc.LoadXml(basketXml);

      XmlNode xnOrderDetail = xdDoc.SelectSingleNode("/ORDER/ORDERDETAIL");
      if (xnOrderDetail != null)
      {
        foreach (XmlAttribute attr in xnOrderDetail.Attributes)
          _orderDetail.Add(attr.Name, attr.Value);
      }

      XmlNode xnDomainContactXML = xdDoc.SelectSingleNode("/ORDER/ORDERDETAIL/DOMAINCONTACTXML");
      if (xnDomainContactXML != null)
        _domainContactXml = xnDomainContactXML.InnerXml;

      XmlNodeList xnlBasketItems = xdDoc.SelectNodes("/ORDER/ITEMS/ITEM");
      foreach (XmlElement xlBasketItem in xnlBasketItems)
        _basketItems.Add(new BasketItem(xlBasketItem));
    }

    public bool IsSuccess
    {
      get { return (_exception == null); }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseXml;
    }

    #endregion

  }
}
