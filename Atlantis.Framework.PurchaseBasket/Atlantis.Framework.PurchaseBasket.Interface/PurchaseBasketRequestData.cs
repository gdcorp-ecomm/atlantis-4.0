using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public class PurchaseBasketRequestData : RequestData
  {
    private List<PurchaseElement> _purchaseElements = new List<PurchaseElement>();
    private List<PaymentElement> _paymentElements = new List<PaymentElement>();
    private Dictionary<string, string> _requestAttributes = new Dictionary<string, string>();

    private string _purchaseXml = null;

    public PurchaseBasketRequestData(string sShopperID,
                                   string sSourceURL,
                                   string sOrderID,
                                   string sPathway,
                                   int iPageCount)
                                   : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
    }

    public void AddPurchaseInfo(PurchaseElement purchaseElement)
    {
      if (purchaseElement != null)
      {
        _purchaseElements.Add(purchaseElement);
      }
    }

    public void AddPayment(PaymentElement paymentElement)
    {
      if (paymentElement != null)
      {
        _paymentElements.Add(paymentElement);
      }
    }

    public void AddPrebuiltPurchaseXml(string purchaseXml)
    {
      _purchaseXml = purchaseXml;
    }

    public void AddRequestAttribute(string name, string value)
    {
      if (!string.IsNullOrEmpty(name))
      {
        _requestAttributes[name] = value;
      }
    }

    private string GenerateXml()
    {
      StringBuilder result = new StringBuilder();
      using (XmlTextWriter xmlWriter = new XmlTextWriter(new StringWriter(result)))
      {
        xmlWriter.WriteStartElement("PaymentInformation");
        foreach (KeyValuePair<string, string> currentAttribute in _requestAttributes)
        {
          xmlWriter.WriteAttributeString(currentAttribute.Key, currentAttribute.Value);
        }
        foreach (PurchaseElement purchaseElement in _purchaseElements)
        {
          if (purchaseElement != null)
          {
            xmlWriter.WriteStartElement(purchaseElement.ElementName);
            foreach (KeyValuePair<string, string> pair in purchaseElement)
            {
              if (!string.IsNullOrEmpty(pair.Key))
              {
                xmlWriter.WriteAttributeString(pair.Key, pair.Value);
              }
            }
            xmlWriter.WriteEndElement();
          }
        }
        
        ProcessPayments(xmlWriter, _paymentElements);        
        
        xmlWriter.WriteEndElement();
      }

      return result.ToString();
    }

    /// <summary>
    /// Place Payments in proper order in the XML
    /// Profile
    /// ISCPayment
    /// GiftCard
    /// CreditLine
    /// CCPayment
    /// ACHPersonall
    /// ACHBusiness
    /// NetGiro
    /// PaypalPayment
    /// PrepaidPayment
    /// ZeroPayment
    /// </summary>
    /// <param name="xmlWriter"></param>
    /// <param name="pymentElements"></param>
    private void ProcessPayments(XmlTextWriter xmlWriter, List<PaymentElement> paymentElements)
    {
      xmlWriter.WriteStartElement("Payments");
      ProcessPayment(xmlWriter, "Profile", paymentElements);
      ProcessPayment(xmlWriter, "ISCPayment", paymentElements);
      ProcessPayment(xmlWriter, "GiftCardPayment", paymentElements);
      ProcessPayment(xmlWriter, "CreditLinePayment", paymentElements);
      ProcessPayment(xmlWriter, "CCPayment", paymentElements);
      ProcessPayment(xmlWriter, "ACHPersonalPayment", paymentElements);
      ProcessPayment(xmlWriter, "ACHBusinessPayment", paymentElements);
      ProcessPayment(xmlWriter, "NetGiroPayment", paymentElements);
      ProcessPayment(xmlWriter, "PaypalPayment", paymentElements);
      ProcessPayment(xmlWriter, "PrepaidPayment", paymentElements);
      ProcessPayment(xmlWriter, "ZeroPayment", paymentElements);
      ProcessPayment(xmlWriter, "ThirdPartyCardPayment", paymentElements);
      xmlWriter.WriteEndElement();
    }

    private void ProcessPayment(XmlTextWriter xmlWriter, string PaymentType, List<PaymentElement> paymentElements)
    {
      foreach (PaymentElement paymentElement in paymentElements)
      {
        if (paymentElement != null && paymentElement.ElementName==PaymentType)
        {
          xmlWriter.WriteStartElement(paymentElement.ElementName);
          foreach (KeyValuePair<string, string> pair in paymentElement)
          {
            if (!string.IsNullOrEmpty(pair.Key))
            {
              xmlWriter.WriteAttributeString(pair.Key, pair.Value);
            }
          }
          xmlWriter.WriteEndElement();
        }
      }
    }

    public override string ToXML()
    {
      string result;
      if (string.IsNullOrEmpty(_purchaseXml))
      {
        result = GenerateXml();
      }
      else
      {
        result = _purchaseXml;
      }
      return result;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("PurchaseBasket is not a cacheable request.");
    }

  }
}
