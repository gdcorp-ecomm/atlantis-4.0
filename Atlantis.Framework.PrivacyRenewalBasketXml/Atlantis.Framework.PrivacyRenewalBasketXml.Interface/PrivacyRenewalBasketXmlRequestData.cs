using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PrivacyRenewalBasketXml.Interface
{
  public class PrivacyRenewalBasketXmlRequestData : RequestData
  {
    public enum PrivateRenewalType
    {
      Privacy,
      Business,
      Protection
    }

    public int PrivacyProductId { get; private set; }
    public PrivateRenewalType RenewalType { get; private set; }
    public int Duration { get; private set; }
    public int DomainBillingResourceId { get; private set; }

    /// <summary>
    /// Default of 30 seconds.
    /// </summary>
    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(30);

    public PrivacyRenewalBasketXmlRequestData(int privacyProductId,
                                             PrivateRenewalType renewalType,
                                             int duration,
                                             int domainBillingResouceId,
                                             string shopperId,
                                             string sourceUrl,
                                             string orderId,
                                             string pathway,
                                             int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PrivacyProductId = privacyProductId;
      RenewalType = renewalType;
      Duration = duration;
      DomainBillingResourceId = domainBillingResouceId;
      RequestTimeout = _requestTimeout;      
    }

    /* <request bundle_pf_id="776001" billing_resource_id="376100" duration="2.000" /> */
    public override string ToXML()
    {
      XmlDocument requestDoc = new XmlDocument();
      requestDoc.LoadXml("<request/>");

      XmlElement oRoot = requestDoc.DocumentElement;

      AddAttribute(oRoot, "bundle_pf_id", PrivacyProductId.ToString());
      AddAttribute(oRoot, "duration", Duration.ToString());
      AddAttribute(oRoot, "billing_resource_id", DomainBillingResourceId.ToString());

      return requestDoc.InnerXml;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("PrivacyRenewalBasketXmlRequestData is not a cacheable request.");
    }

    private static void AddAttribute(XmlNode node, string attributeName, string attributeValue)
    {
      XmlAttribute attribute = node.OwnerDocument.CreateAttribute(attributeName);
      node.Attributes.Append(attribute);
      attribute.Value = attributeValue;
    }
  }
}
