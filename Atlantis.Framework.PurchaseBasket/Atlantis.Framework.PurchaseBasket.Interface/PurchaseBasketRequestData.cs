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

        xmlWriter.WriteStartElement("Payments");

        foreach (PaymentElement paymentElement in _paymentElements)
        {
          if (paymentElement != null)
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

        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
      }

      return result.ToString();
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
