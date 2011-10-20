using System;
using System.Reflection;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SmartSpaceRenewalBasketXml.Interface
{
  public class SmartSpaceRenewalBasketXmlResponseData : IResponseData
  {
    private SmartSpaceRenewalBasketXmlRequestData RequestData { get; set; }
    private AtlantisException Exception { get; set; }

    public string RenewalXml { get; private set; }
    /// <summary>
    /// This is the resource_id that needs to be an attribute on the cart <ITEM /> element 
    /// </summary>
    public string CartItemResourceId { get; private set; }
    public bool IsSuccess { get; private set; }

    internal SmartSpaceRenewalBasketXmlResponseData(int smartDomainResourceId, RequestData requestData)
    {
      RequestData = requestData as SmartSpaceRenewalBasketXmlRequestData;

      if(smartDomainResourceId > 0)
      {
        CartItemResourceId = smartDomainResourceId.ToString();
        RenewalXml = BuildSmartSpaceRenewalXml();
        IsSuccess = true;
      }
      else
      {
        CartItemResourceId = "0";
        RenewalXml = string.Empty;
        IsSuccess = false;
      }
    }

    internal SmartSpaceRenewalBasketXmlResponseData(AtlantisException ex, RequestData requestData)
    {
      RequestData = requestData as SmartSpaceRenewalBasketXmlRequestData;
      IsSuccess = false;
      Exception = ex;
    }

    internal SmartSpaceRenewalBasketXmlResponseData(Exception ex, RequestData requestData)
    {
      RequestData = requestData as SmartSpaceRenewalBasketXmlRequestData;
      IsSuccess = false;
      Exception = new AtlantisException(requestData,
                                        MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                        ex.Message,
                                        string.Format("BillingResourceId:{0},Sld:{1},Tld:{2}", RequestData.BillingResourceId, RequestData.Sld, RequestData.Tld));
    }

    private string BuildSmartSpaceRenewalXml()
    {
      XmlDocument smartSpaceRenewalDoc = new XmlDocument();
      smartSpaceRenewalDoc.LoadXml("<smartDomainRenewal/>");

      XmlElement domainElement = smartSpaceRenewalDoc.CreateElement("domain");
      AddAttribute(domainElement, "sld", RequestData.Sld);
      AddAttribute(domainElement, "tld", RequestData.Tld);
      AddAttribute(domainElement, "resourceid", CartItemResourceId);
      AddAttribute(domainElement, "duration", RequestData.Duration.ToString());

      smartSpaceRenewalDoc.DocumentElement.AppendChild(domainElement);

      return smartSpaceRenewalDoc.InnerXml;
    }

    private static void AddAttribute(XmlNode node, string attributeName, string attributeValue)
    {
      XmlAttribute attribute = node.OwnerDocument.CreateAttribute(attributeName);
      node.Attributes.Append(attribute);
      attribute.Value = attributeValue;
    }

    /*
      <smartDomainRenewal>
        <domain sld='THISISADEVFULLFILLTRWALKER' tld='ORG' resourceid='1665574' duration='2'/>
      </smartDomainRenewal>
    */
    public string ToXML()
    {
      return RenewalXml;
    }

    public AtlantisException GetException()
    {
      return Exception;
    }
  }
}
