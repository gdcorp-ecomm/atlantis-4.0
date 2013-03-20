using System;
using Atlantis.Framework.Interface;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.EcommValidPaymentType.Interface
{
  public class EcommValidPaymentTypeRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(20);

    public string BasketType { get; set; }

    /// <summary>
    /// This is the transactional currency the cart has in the cart xml
    /// </summary>
    public string TransactionalCurrencyType { get; set; }
    public string SelectedCountry { get; set; }
    public string RestrictedPaymentTypes { get; set; }
    public int ShopperPaymentTypeID { get; set; }

    public EcommValidPaymentTypeRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string basketType, string transactionalCurrencyType) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      BasketType = basketType;
      TransactionalCurrencyType = transactionalCurrencyType;
      RequestTimeout = _defaultRequestTimeout;
      ShopperPaymentTypeID = -1;
    }

    public EcommValidPaymentTypeRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string basketType, string transactionalCurrencyType,string selectedCountry)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      BasketType = basketType;
      TransactionalCurrencyType = transactionalCurrencyType;
      SelectedCountry = selectedCountry;
      RequestTimeout = _defaultRequestTimeout;
      ShopperPaymentTypeID = -1;
    }

    public override string ToXML()
    {
      System.Text.StringBuilder responseXML = new System.Text.StringBuilder(100);
      XmlTextWriter customXmlWriter = new XmlTextWriter(new StringWriter(responseXML));
      customXmlWriter.WriteStartElement("GetActivePaymentTypes");
      customXmlWriter.WriteStartElement("FilterBy");
      customXmlWriter.WriteAttributeString("currency", TransactionalCurrencyType);
      customXmlWriter.WriteAttributeString("countryCode", SelectedCountry);
      if (!string.IsNullOrEmpty(RestrictedPaymentTypes))
      {
        customXmlWriter.WriteAttributeString("restrictedPaymentType", RestrictedPaymentTypes);
      }
      if (ShopperPaymentTypeID != -1)
      {
        customXmlWriter.WriteAttributeString("shopperPaymentTypeID", ShopperPaymentTypeID.ToString());
      }
      customXmlWriter.WriteEndElement();
      customXmlWriter.WriteEndElement();
      return responseXML.ToString();
    }

    public override string GetCacheMD5()
    {
      throw new Exception("EcommValidPaymentTypeRequestData is not a cacheable request.");
    }
  }
}
