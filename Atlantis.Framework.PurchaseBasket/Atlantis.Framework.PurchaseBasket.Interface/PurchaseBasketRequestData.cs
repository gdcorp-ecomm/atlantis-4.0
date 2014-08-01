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

    
    private Dictionary<string, string> _instantAttributes = new Dictionary<string, string>();
    private List<string> _instantItemElements = new List<string>();

    private string _purchaseXml = null;

    public PurchaseBasketRequestData(string sShopperID,
                                   string sSourceURL,
                                   string sOrderID,
                                   string sPathway,
                                   int iPageCount)
                                   : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
    }

    public Dictionary<string, string> RequestAttributes
    {
      get
      {
        return _requestAttributes;
      }
    }

    public Dictionary<string, string> InstantPurchaseAttributes
    {
      get
      {
        return _instantAttributes;
      }
    }

    public List<string> InstantItemElements
    {
      get
      {
        return _instantItemElements;
      }
    }

    public List<PurchaseElement> PurchaseElements
    {
      get
      {
        return _purchaseElements;
      }
    }
    
    public List<PaymentElement> PaymentElements
    {
      get
      {
        return _paymentElements;
      }
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

    public void AddInstantPurchaseAttribute(string name, string value)
    {
      if (!string.IsNullOrEmpty(name))
      {
        _instantAttributes[name] = value;
      }
    }

    public void AddInstantPurchaseItem(string itemXML)
    {
      if (_instantItemElements != null)
      {
        _instantItemElements.Add(itemXML);
      }
    }

    private List<PaymentElement> AllPaymentTypes()
    {
      List<PaymentElement> allElements = new List<PaymentElement>();
      allElements.Add(new PaymentUseACHBusiness());
      allElements.Add(new PaymentUseACHPersonal());
      allElements.Add(new PaymentUseCreditCard());
      allElements.Add(new PaymentUseGiftCard());
      allElements.Add(new PaymentUseInStoreCredit());
      allElements.Add(new PaymentUseLineOfCredit());
      allElements.Add(new PaymentUseNetGiro());
      allElements.Add(new PaymentUsePaypal());
      allElements.Add(new PaymentUsePaypal());
      allElements.Add(new PaymentUseProfile());
      allElements.Add(new PaymentUsePrepaid());
      allElements.Add(new PaymentUseThirdPartyCard());
      allElements.Add(new PaymentUseZero());
      return allElements;
    }

    public void PopulateRequestFromXML(string purchaseXML)
    {      
      XmlDocument purchaseDoc = new XmlDocument();
      purchaseDoc.LoadXml(purchaseXML);
      //Parse-process Purchase Nodes
      XmlNode attributes = purchaseDoc.SelectSingleNode("//PaymentInformation");
      foreach (XmlAttribute xmlAttr in attributes.Attributes)
      {
        _requestAttributes[xmlAttr.Name] = xmlAttr.Value;
      }

      XmlNode instantAttributes = purchaseDoc.SelectSingleNode("//InstantPurchase");
      if (instantAttributes != null && instantAttributes.Attributes != null)
      {
        foreach (XmlAttribute xmlAttr in instantAttributes.Attributes)
        {
          _instantAttributes[xmlAttr.Name] = xmlAttr.Value;
        }
      }

      XmlNode items = purchaseDoc.SelectSingleNode("//InstantPurchase/itemRequest");
      if (items != null && items.HasChildNodes)
      {
        foreach (XmlNode itemNode in items.ChildNodes)
        {
          _instantItemElements.Add(itemNode.OuterXml);
        }
      }

      XmlNode billingInfo = purchaseDoc.SelectSingleNode("//PaymentInformation/BillingInfo");
      PurchaseBillingInfo oBilling = new PurchaseBillingInfo();
      oBilling.PopulateFromXML(billingInfo);
      _purchaseElements.Add(oBilling);

      XmlNode purchaseInfo = purchaseDoc.SelectSingleNode("//PaymentInformation/PaymentOrigin");
      PurchasePaymentOrigin origin = new PurchasePaymentOrigin();
      origin.PopulateFromXML(purchaseInfo);
      _purchaseElements.Add(origin);     

      XmlNode payments = purchaseDoc.SelectSingleNode("//PaymentInformation/Payments");
      List<PaymentElement> allPaymentTypes=AllPaymentTypes();

      foreach (XmlNode currentNode in payments.ChildNodes)
      {
        foreach (PaymentElement currentElement in allPaymentTypes)
        {
          if (currentElement.ElementName == currentNode.Name)
          {
            currentElement.Clear();
            currentElement.PopulateFromXML(currentNode);
            PaymentElement newElement=currentElement.Clone();
            _paymentElements.Add(newElement);
            break;
          }
        }
      }
    }

    private string GenerateXml()
    {
      StringBuilder result = new StringBuilder();
      bool isInstantPurchase = (_instantItemElements != null && _instantItemElements.Count > 0);

      using (XmlTextWriter xmlWriter = new XmlTextWriter(new StringWriter(result)))
      {
        if (isInstantPurchase)
        {
          xmlWriter.WriteStartElement("PurchaseBasketWithItems");
        }

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

        if (isInstantPurchase)
        {
          xmlWriter.WriteStartElement("InstantPurchase");
          foreach (KeyValuePair<string, string> currentAttribute in _instantAttributes)
          {
            xmlWriter.WriteAttributeString(currentAttribute.Key, currentAttribute.Value);
          }

          xmlWriter.WriteStartElement("itemRequest");
          foreach (string item in _instantItemElements)
          {
            xmlWriter.WriteRaw(item);
          }
          xmlWriter.WriteEndElement(); // itemRequest

          xmlWriter.WriteEndElement(); // PurchaseBasketWithItems
        }     
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
