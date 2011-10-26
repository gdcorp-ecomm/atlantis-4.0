using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PrivacyRenewalBasketXml.Interface
{
  public class PrivacyRenewalBasketXmlResponseData : IResponseData
  {
    private AtlantisException Exception { get; set; }

    public string RenewalXml { get; private set; }
    /// <summary>
    /// This is the resource_id that needs to be an attribute on the cart <ITEM /> element 
    /// </summary>
    public string CartItemResourceId { get; private set; }
    public bool IsSuccess { get; private set; }

    internal PrivacyRenewalBasketXmlResponseData(string responseXml, RequestData requestData, string errorXml)
    {
      PrivacyRenewalBasketXmlRequestData renewalBundleXmlGetRequestData = requestData as PrivacyRenewalBasketXmlRequestData;
      IsSuccess = false;
 
      if(renewalBundleXmlGetRequestData != null)
      {
        if(!string.IsNullOrEmpty(errorXml))
        {
          Exception = new AtlantisException(requestData,
                                            "PrivacyRenewalBasketXmlResponseData",
                                            errorXml,
                                            requestData.ToXML());
        }
        else
        {
          RenewalXml = ParseBundleXml(responseXml, renewalBundleXmlGetRequestData);
          IsSuccess = true;
        }
      }
    }

    internal PrivacyRenewalBasketXmlResponseData(string responseXml, int parentBundleId, RequestData requestData, string errorXml) : this(responseXml, requestData, errorXml)
    {
      CartItemResourceId = parentBundleId.ToString();
    }

    internal PrivacyRenewalBasketXmlResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      RenewalXml = responseXml;
      IsSuccess = false;
      Exception = new AtlantisException(requestData,
                                         "PrivacyRenewalBasketXmlResponseData",
                                         ex.Message,
                                         requestData.ToXML());
    }

    private string ParseBundleXml(string responseXML, PrivacyRenewalBasketXmlRequestData requestData)
    {
      string parsedXml = string.Empty;

      switch (requestData.RenewalType)
      {
        case PrivacyRenewalBasketXmlRequestData.PrivateRenewalType.Privacy:
          parsedXml = ParseProxyXml(responseXML);
          break;
        case PrivacyRenewalBasketXmlRequestData.PrivateRenewalType.Business:
          parsedXml = ParseProximaXml(responseXML);
          break;
        case PrivacyRenewalBasketXmlRequestData.PrivateRenewalType.Protection:
          // We return the xml as is.  The web service already returns the full protection bundle xml.
          parsedXml = responseXML;
          break;
      }

      return parsedXml;
    }

    private string ParseProxyXml(string responseXML)
    {
      string xml = string.Empty;

      XmlDocument responseDocument = new XmlDocument();
      responseDocument.LoadXml(responseXML);

      XmlNode proxyNode = responseDocument.SelectSingleNode("//domainByProxyBulkRenewal");

      if (proxyNode != null && proxyNode.FirstChild != null)
      {
        if (proxyNode.Attributes["dbpuser"] == null)
        {
          proxyNode.Attributes.Append(responseDocument.CreateAttribute("dbpuser", ""));
        }

        if (proxyNode.Attributes["new"] == null)
        {
          proxyNode.Attributes.Append(responseDocument.CreateAttribute("new", ""));
        }

        xml = proxyNode.OuterXml;

        XmlAttribute resourceAttr = proxyNode.FirstChild.Attributes["resourceid"];
        if (resourceAttr != null && !string.IsNullOrEmpty(resourceAttr.Value))
        {
          CartItemResourceId = resourceAttr.Value.Replace("domain:", string.Empty);
        }
      }

      return xml;
    }

    private string ParseProximaXml(string responseXML)
    {
      string xml = string.Empty;

      XmlDocument responseDocument = new XmlDocument();
      responseDocument.LoadXml(responseXML);

      XmlNode proximaNode = responseDocument.SelectSingleNode("//ProximaRenewal");
      if(proximaNode != null && proximaNode.FirstChild != null)
      {
        xml = proximaNode.OuterXml;

        XmlAttribute resourceAttr = proximaNode.FirstChild.Attributes["resource_id"];
        if (resourceAttr != null && !string.IsNullOrEmpty(resourceAttr.Value))
        {
          CartItemResourceId = resourceAttr.Value;
        }
      }

      return xml;
    }

    /*
     Privacy Renewal XML:
      <domainByProxyBulkRenewal dbpuser="" new="">
        <domain sld='THISISADEVFULLFILLTRWALKER' tld='ORG' resourceid='domain:1665574' duration='2.000'/>
      </domainByProxyBulkRenewal>
     
     Business Renewal XML:
      <ProximaRenewal>
        <domain sld='THISISADEVFULLFILLTRWALKER' tld='ORG' resource_id='376104' duration='2.000'/>
      </ProximaRenewal>
     
     Protection Renewal XML:
      <BUNDLE>
        <BUNDLEITEM index='1'>
          <domainByProxyBulkRenewal dbpuser="" new="">
            <domain sld='THISISADEVFULLFILLTRWALKER' tld='ORG' resourceid='domain:1665574' duration='2.000'/>
          </domainByProxyBulkRenewal>
        </BUNDLEITEM>
        <BUNDLEITEM index='2'>
          <ProximaRenewal>
            <domain sld='THISISADEVFULLFILLTRWALKER' tld='ORG' resource_id='376104' duration='2.000'/>
          </ProximaRenewal>
        </BUNDLEITEM>
        <BUNDLEITEM index='3'>
          <expirationProtectionRenewal>
            <domain sld='THISISADEVFULLFILLTRWALKER' tld='ORG' resource_id='376106' duration='2.000'/>
          </expirationProtectionRenewal>
        </BUNDLEITEM>
      </BUNDLE>
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
