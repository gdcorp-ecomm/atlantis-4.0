using System;
using System.Xml.Linq;

using Atlantis.Framework.AddItem.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ExpressCheckoutPurchase.Interface
{
  public class ExpressCheckoutPurchaseRequestData : RequestData
  {
    public XDocument WebServiceRequestXml { get; private set; }

    #region Constructors
    public ExpressCheckoutPurchaseRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string requestingAppHost,
                                  string clientIP,
                                  bool sendConfirmEmail,
                                  string enteredBy,
                                  string orderSource,
                                  AddItemRequestData itemRequestData)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      
      WebServiceRequestXml = new XDocument(
        new XElement("instantPurchase",
          new XAttribute("requestingApp", string.Empty),
          new XAttribute("requestingAppHost", requestingAppHost),
          new XAttribute("clientAddr", clientIP),
          new XAttribute("sendConfimEmail", Convert.ToInt32(sendConfirmEmail).ToString()),
          new XAttribute("entered_by", enteredBy),
          new XAttribute("order_source", orderSource)
        )
      );

      try
      {
        XDocument itemRequestDoc = XDocument.Parse(itemRequestData.ToXML());
        WebServiceRequestXml.Element("instantPurchase").Add(itemRequestDoc.Element("itemRequest"));
      }
      catch (Exception ex)
      {
        throw new AtlantisException(this, "ExpressCheckoutPurchaseRequestData::ExpressCheckoutPurchaseRequestData", ex.Message, ex.StackTrace);
      }

      RequestTimeout = TimeSpan.FromSeconds(5d);
    }

    public ExpressCheckoutPurchaseRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string requestingAppHost,
                                  string clientIP,
                                  bool sendConfirmEmail,
                                  string enteredBy,
                                  string orderSource,
                                  AddItemRequestData itemRequestData,
                                  string expectedTotalInPennies,
                                  bool estimateOnly,
                                  string transactionCurrency)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      WebServiceRequestXml = new XDocument(
        new XElement("instantPurchase",
          new XAttribute("requestingApp", string.Empty),
          new XAttribute("requestingAppHost", requestingAppHost),
          new XAttribute("clientAddr", clientIP),
          new XAttribute("sendConfimEmail", Convert.ToInt32(sendConfirmEmail).ToString()),
          new XAttribute("entered_by", enteredBy),
          new XAttribute("order_source", orderSource),
          new XAttribute("expectedTotal", expectedTotalInPennies),
          new XAttribute("estimateOnly", Convert.ToInt32(estimateOnly).ToString()),
          new XAttribute("transactionCurrency", transactionCurrency)
        )
      );

      try
      {
        XDocument itemRequestDoc = XDocument.Parse(itemRequestData.ToXML());
        WebServiceRequestXml.Element("instantPurchase").Add(itemRequestDoc.Element("itemRequest"));
      }
      catch (Exception ex)
      {
        throw new AtlantisException(this, "ExpressCheckoutPurchaseRequestData::ExpressCheckoutPurchaseRequestData", ex.Message, ex.StackTrace);
      }

      RequestTimeout = TimeSpan.FromSeconds(5d);
    }
    #endregion


    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in ExpressCheckoutPurchaseRequestData");     
    }

    public override string ToXML()
    {
      return WebServiceRequestXml.ToString();
    }
    #endregion
  }
}
