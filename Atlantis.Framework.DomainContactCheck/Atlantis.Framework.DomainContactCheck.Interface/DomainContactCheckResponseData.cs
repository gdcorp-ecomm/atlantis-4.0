using System;
using System.Collections.Generic;
using System.Xml;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainContactCheck.Interface
{
  public class DomainContactCheckResponseData : IResponseData
  {
    public Dictionary<string, string> TrusteeVendorIds { get; private set; }
    public List<DomainContactError> Errors { get; private set; }
    public XmlAttributeCollection ResponseAttributes { get; private set; }

    public string ContactXml
    {
      get
      {
        var xdDoc = new XmlDocument();
        xdDoc.LoadXml(m_sResponseXML);
        XmlNode xmlContact = xdDoc.SelectSingleNode("//contact");
        return (xmlContact.InnerXml);
      }
    }
    
    public DomainContactCheckResponseData(string domainXml)
    {
      Errors = new List<DomainContactError>();
      TrusteeVendorIds = new Dictionary<string,string>();
      m_sResponseXML = domainXml;
      PopulateFromXml();
    }

    public DomainContactCheckResponseData(AtlantisException exAtlantis)
    {
      Errors = new List<DomainContactError>();
      TrusteeVendorIds = new Dictionary<string,string>();
      m_sResponseXML = string.Empty;
      m_ex = exAtlantis;
      PopulateFromXml();
    }

    public DomainContactCheckResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      Errors = new List<DomainContactError>();
      TrusteeVendorIds = new Dictionary<string,string>();
      m_sResponseXML = responseXml;
      m_ex = new AtlantisException(oRequestData,
                                   "DomainContactCheckResponseData",
                                   ex.Message,
                                   string.Empty);
    
      PopulateFromXml();
    }

    private void PopulateFromXml()
    {
      var xdDoc = new XmlDocument();
      xdDoc.LoadXml(m_sResponseXML);

      var contactNode = xdDoc.SelectSingleNode("/contact") as XmlElement;
      string contactTypeValue = contactNode.GetAttribute(DomainContactAttributes.DomainContactType);
      int contactType;
      if (contactTypeValue.Equals("billing", StringComparison.OrdinalIgnoreCase))
        contactType = 3;
      else if (contactTypeValue.Equals("technical", StringComparison.OrdinalIgnoreCase))
        contactType = 1;
      else if(contactTypeValue.Equals("administrative", StringComparison.OrdinalIgnoreCase))
        contactType = 2;
      else
        contactType =0;
      ResponseAttributes = contactNode.Attributes;

      XmlNodeList xnlErrors = xdDoc.SelectNodes("/contact/error");

      foreach (XmlElement xlError in xnlErrors)
      { 
        string sAttribute = xlError.GetAttribute("attribute");
        int iCode;
        int.TryParse(xlError.GetAttribute("code"), out iCode);
        string sDescription = xlError.GetAttribute("desc");
        string sDisplayString = xlError.GetAttribute("displaystring");
        string sDotType = xlError.GetAttribute("tld");

        Errors.Add( new DomainContactError(sAttribute, iCode, sDescription, sDisplayString, sDotType, contactType));
      }

      XmlNodeList trusteeNodes = xdDoc.SelectNodes("/contact/trustee");

      foreach (XmlElement element in trusteeNodes)
      {
        string dotType = element.GetAttribute("tld");
        string vendorId = element.GetAttribute("vendorid");
        TrusteeVendorIds[dotType] = vendorId;
      }
    }

    public bool IsSuccess
    {
      get { return m_sResponseXML.IndexOf("success", StringComparison.OrdinalIgnoreCase) > -1; }
    }

    #region IResponseData Members

    private readonly AtlantisException m_ex;
    public AtlantisException GetException()
    {
      return m_ex;
    }

    private readonly string m_sResponseXML;
    public string ToXML()
    {
      return m_sResponseXML;
    }

    #endregion
  }
}
