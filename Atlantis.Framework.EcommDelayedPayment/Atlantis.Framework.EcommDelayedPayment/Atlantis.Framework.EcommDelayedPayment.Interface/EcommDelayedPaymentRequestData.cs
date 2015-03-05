using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.EcommDelayedPayment.Interface
{
  public class EcommDelayedPaymentRequestData : RequestData
  {

    public string ReturnURL { get; set; }
    public string RequestType { get; set; }
    public BillingInfo Billing{get;set;}
    public PaymentInfo Payment { get; set; }
    public Payments Payments { get; set; }
    public string MarketID { get; set; }

    public EcommDelayedPaymentRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  string returnURL, string requestType)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(40);
      ReturnURL = returnURL;
      RequestType = requestType;
      Billing = new BillingInfo();
      Payment = new PaymentInfo();
      Payments = new Payments();
      MarketID = string.Empty;
    }

    public EcommDelayedPaymentRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  string returnURL, string requestType,string marketID)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(40);
      ReturnURL = returnURL;
      RequestType = requestType;
      Billing = new BillingInfo();
      Payment = new PaymentInfo();
      Payments = new Payments();
      MarketID = marketID;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EcommDelayedPaymentRequestData");
    }


    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("DelayedPurchase");
      xtwRequest.WriteAttributeString("shopper_id", ShopperID);
      xtwRequest.WriteAttributeString("type", RequestType);
      xtwRequest.WriteAttributeString("return_url", ReturnURL);
      if (!string.IsNullOrEmpty(MarketID))
      {
        xtwRequest.WriteAttributeString("MarketID", MarketID);
      }
      Payment.ToXML(xtwRequest);
      Billing.ToXML(xtwRequest);
      Payments.ToXML(xtwRequest);
      xtwRequest.WriteEndElement();
      System.Diagnostics.Debug.WriteLine(sbRequest.ToString());
      return sbRequest.ToString();

    }
  }
}
