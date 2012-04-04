using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.MYARecentOrders.Interface
{
  public class MYARecentOrdersResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;
    private List<RecentOrder> _recentOrders;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public List<RecentOrder> RecentOrders
    {
      get { return _recentOrders; }
    }

    public MYARecentOrdersResponseData()
    {
    }

    public MYARecentOrdersResponseData(IEnumerable<RecentOrder> recentOrders)
    {
      _recentOrders = new List<RecentOrder>(recentOrders);
      _success = true;
    }

    public MYARecentOrdersResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public MYARecentOrdersResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "MYARecentOrdersResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      XDocument resultDoc = new XDocument();
      XElement resultRoot = new XElement("orders");
      resultDoc.Add(resultRoot);

      foreach (RecentOrder ro in RecentOrders)
      {
        resultRoot.Add(new XElement("order",
          new XAttribute("dateEntered", ro.DateEntered.ToString()),
          new XAttribute("isRefund", ro.IsRefund.ToString()),
          new XAttribute("orderId", ro.OrderId),
          new XAttribute("orderSource", ro.OrderSource),
          new XAttribute("shopperId", ro.ShopperId),
          new XAttribute("transactionTotal", ro.TransactionTotal.ToString()),
          new XAttribute("transactionCurrency", ro.TransactionCurrency)));
      }
      return resultDoc.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      _recentOrders = new List<RecentOrder>();

      if (!string.IsNullOrEmpty(sessionData))
      {
        XmlDocument xdoc = new XmlDocument();
        xdoc.LoadXml(sessionData);
        XmlNodeList recentOrderNodes = xdoc.SelectNodes("orders/order");

        if (recentOrderNodes != null)
        {
          foreach (XmlNode node in recentOrderNodes)
          {
            RecentOrder ro = new RecentOrder();

            ro.DateEntered = Convert.ToDateTime(node.Attributes["dateEntered"].Value);
            ro.IsRefund = Convert.ToBoolean(node.Attributes["isRefund"].Value);
            ro.OrderId = Convert.ToString(node.Attributes["orderId"].Value);
            ro.OrderSource = Convert.ToString(node.Attributes["orderSource"].Value);
            ro.ShopperId = Convert.ToString(node.Attributes["shopperId"].Value);
            ro.TransactionTotal = Convert.ToInt32(node.Attributes["transactionTotal"].Value);
            ro.TransactionCurrency = Convert.ToString(node.Attributes["transactionCurrency"].Value);

            _recentOrders.Add(ro);
          }
        }
      }
      _success = true;
    }

    #endregion
  }
}
