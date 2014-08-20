using Atlantis.Framework.DCCGetDomainByShopper.Interface.Paging;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.DCCGetDomainByShopper.Interface
{
  [DataContract]
  public class DCCGetDomainByShopperResponseData : IResponseData, ISessionSerializableResponse
  {
    private readonly AtlantisException _exception;

    [DataMember]
    public string ResponseXml { get; set; }

    [DataMember]
    public bool UseMaxdateAsDefaultForExpirationDate { get; set; }

    public FullSummary FullSummary { get; private set; }

    public PageSummary PageSummary { get; private set; }

    public IDictionary<string, IDictionary<string, string>> Domains { get; private set; }

    public bool IsSuccess { get; private set; }

    public DCCGetDomainByShopperResponseData()
    {
    }

    public DCCGetDomainByShopperResponseData(string responseXml, bool useMaxdateAsDefaultForExpirationDate)
    {
      ResponseXml = responseXml;
      UseMaxdateAsDefaultForExpirationDate = useMaxdateAsDefaultForExpirationDate;
      Domains = new Dictionary<string, IDictionary<string, string>>();
      PopulateFromXml(responseXml);
      IsSuccess = true;
    }

    public DCCGetDomainByShopperResponseData(string responseXml, AtlantisException exAtlantis)
    {
      ResponseXml = responseXml;
      _exception = exAtlantis;
      IsSuccess = false;
    }

    public DCCGetDomainByShopperResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      ResponseXml = responseXml;
      _exception = new AtlantisException("DCCGetDomainByShopperResponseData", 1, ex.Message, oRequestData.ToXML());
      IsSuccess = false;
    }

    /*
    <results>
      <method>GetDCCDomainList</method>
      <success>1</success>
      <searchsummary resultcount="378" firstdomain="0kajsdhfjkahsdf.com" lastdomain="yourcarinapro.com"></searchsummary>
      <domains>
        <domain id="1664532" domainname="ADSLJKFHASLD.COM" expirationdate="05/15/2010 15:39:03" sortexpirationdate="05/15/2010 00:00:00" status="0" parent_bundle_id="" islocked="1" isproxied="1" islimited="0" tldid="1" guid="{628A20F2-4190-11DE-BAA9-005056956427}" extendedwhoistype="0" isexpirationprotected="0" istransferprotected="0" isregistrarhold="" autorenewflag="1" deletedate="" renewperiod="0" profileid="" vendorid="" issmartdomain="0" billingresourceid="367943" profilename="" invalidwhois="" certifieddomainstatus="" dscor_resultoverall=""></domain>
      </domains>
    </results>
    */

    private void PopulateFromXml(string basketXml)
    {
      XmlDocument xdDoc = new XmlDocument();
      xdDoc.LoadXml(basketXml);

      XmlElement xnSuccess = (XmlElement)xdDoc.SelectSingleNode("/results/success");
      if (xnSuccess != null && xnSuccess.InnerText == "1")
      {
        IsSuccess = true;
      }

      XmlElement xnSummary = (XmlElement)xdDoc.SelectSingleNode("/results/searchsummary");
      if (xnSuccess != null && xnSummary != null)
      {
        int resultCount = Convert.ToInt32(xnSummary.Attributes["resultcount"].Value );
        string firstDomain = xnSummary.Attributes["firstdomain"].Value;
        string lastDomain = xnSummary.Attributes["lastdomain"].Value;

        FullSummary = new FullSummary { ResultCount = resultCount, FirstDomain = firstDomain, LastDomain = lastDomain };
      }

      XmlNodeList domainsNodeList = xdDoc.SelectNodes("/results/domains/domain");

      if (domainsNodeList != null)
      {
        foreach (XmlElement domainElement in domainsNodeList)
        {
          if (domainElement.Attributes[DomainAttibutes.DomainName] != null && !Domains.ContainsKey(domainElement.Attributes[DomainAttibutes.DomainName].Value))
          {
            IDictionary<string, string> domainAttributeDictionary = new Dictionary<string, string>(domainElement.Attributes.Count);

            foreach (XmlAttribute domainAttribute in domainElement.Attributes)
            {
              if (!domainAttributeDictionary.ContainsKey(domainAttribute.Name))
              {
                domainAttributeDictionary.Add(domainAttribute.Name, domainAttribute.Value);
              }
            }

            if(!domainAttributeDictionary.ContainsKey(DomainAttibutes.Xml))
            {
              domainAttributeDictionary.Add(DomainAttibutes.Xml, domainElement.OuterXml);
            }

            Domains.Add(domainElement.Attributes[DomainAttibutes.DomainName].Value, domainAttributeDictionary);
          }
        }

        SetPageSummary();
      }     
    }

    private void  SetPageSummary()
    {
      if (Domains.Count > 0)
      {
        string firstDomainName, lastDomainName;
        string firstExpirationDateString, lastExpirationDateString;
        DateTime firstExpirationDate = UseMaxdateAsDefaultForExpirationDate ? DateTime.MaxValue : DateTime.MinValue;
        DateTime lastExpirationDate = UseMaxdateAsDefaultForExpirationDate ? DateTime.MaxValue : DateTime.MinValue;

        IDictionary<string, string> firstDomain = Domains.Values.First();
        firstDomain.TryGetValue("domainname", out firstDomainName);
        
        if (firstDomain.TryGetValue("expirationdate", out firstExpirationDateString))
        {
          if (!DateTime.TryParse(firstExpirationDateString, out firstExpirationDate))
          {
            firstExpirationDate = UseMaxdateAsDefaultForExpirationDate ? DateTime.MaxValue : DateTime.MinValue;
          }
        }

        IDictionary<string, string> lastDomain = Domains.Values.Last();
        lastDomain.TryGetValue("domainname", out lastDomainName);
        
        if (lastDomain.TryGetValue("expirationdate", out lastExpirationDateString))
        {
          if (!DateTime.TryParse(lastExpirationDateString, out lastExpirationDate))
          {
            lastExpirationDate = UseMaxdateAsDefaultForExpirationDate ? DateTime.MaxValue : DateTime.MinValue;
          }
        }

        PageSummary = new PageSummary { FirstDomainName = firstDomainName, FirstExpirationDate = firstExpirationDate, LastDomainName = lastDomainName, LastExpirationDate = lastExpirationDate };
      }
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    
    public string ToXML()
    {
      return ResponseXml;
    }

    public string SerializeSessionData()
    {
      string serialized;

      try
      {
        DataContractJsonSerializer json = new DataContractJsonSerializer(typeof (DCCGetDomainByShopperResponseData));
        using (MemoryStream ms = new MemoryStream())
        {
          json.WriteObject(ms, this);
          ms.Position = 0;
          serialized = new StreamReader(ms).ReadToEnd();
        }
      }
      catch
      {
        serialized = string.Empty;
      }

      return serialized;
    }

    public void DeserializeSessionData(string sessionData)
    {
      if (!string.IsNullOrEmpty(sessionData))
      {
        try
        {
          byte[] ba = Encoding.ASCII.GetBytes(sessionData);
          using (MemoryStream ms = new MemoryStream(ba))
          {
            DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(DCCGetDomainByShopperResponseData));
            DCCGetDomainByShopperResponseData result = (DCCGetDomainByShopperResponseData)dcs.ReadObject(ms);
            if(result != null)
            {
              UseMaxdateAsDefaultForExpirationDate = result.UseMaxdateAsDefaultForExpirationDate;
              Domains = new Dictionary<string, IDictionary<string, string>>();
              PopulateFromXml(result.ResponseXml);
              IsSuccess = true;
            }
          }
        }
        catch
        {
          Domains = new Dictionary<string, IDictionary<string, string>>(1);
          IsSuccess = false;
          throw; // Rethrow exception so SessionCache logs the exception
        }
      }
    }
  }
}
