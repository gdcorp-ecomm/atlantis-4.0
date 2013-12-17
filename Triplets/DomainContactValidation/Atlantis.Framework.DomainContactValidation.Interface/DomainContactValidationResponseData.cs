using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainContactValidation.Interface
{
  public class DomainContactValidationResponseData : IResponseData
  {
    public Dictionary<string, string> TrusteeVendorIds { get; private set; }
    public List<DomainContactValidationError> Errors { get; private set; }
    public XmlAttributeCollection ResponseAttributes { get; private set; }
    
    public string ContactXml
    {
      get
      {
        var result = string.Empty;

        var xdDoc = new XmlDocument();
        if (!string.IsNullOrEmpty(_responseXml))
        {
          xdDoc.LoadXml(_responseXml);
          XmlNode xmlContact = xdDoc.SelectSingleNode("//contact");
          if (xmlContact != null)
          {
            result = xmlContact.InnerXml;
          }
        }

        return result;
      }
    }

    public DomainContactValidationResponseData(string domainXml)
    {
      Errors = new List<DomainContactValidationError>();
      TrusteeVendorIds = new Dictionary<string, string>();
      _responseXml = domainXml;
      PopulateFromXml();
    }

    public DomainContactValidationResponseData(AtlantisException exAtlantis)
    {
      Errors = new List<DomainContactValidationError>();
      TrusteeVendorIds = new Dictionary<string, string>();
      _responseXml = string.Empty;
      _ex = exAtlantis;
      PopulateFromXml();
    }

    public DomainContactValidationResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      Errors = new List<DomainContactValidationError>();
      TrusteeVendorIds = new Dictionary<string, string>();
      _responseXml = responseXml;
      _ex = new AtlantisException(oRequestData,
                                   "DomainContactValidationResponseData",
                                   ex.Message,
                                   string.Empty);

      PopulateFromXml();
    }

    private void PopulateFromXml()
    {
      if (!string.IsNullOrEmpty(_responseXml))
      {
        var xdDoc = new XmlDocument();
        xdDoc.LoadXml(_responseXml);

        var contactNode = xdDoc.SelectSingleNode("/contact") as XmlElement;
        if (contactNode != null)
        {
          string contactTypeValue = contactNode.GetAttribute(DomainContactAttributes.DomainContactType);
          int contactType;
          if (contactTypeValue.Equals("billing", StringComparison.OrdinalIgnoreCase))
          {
            contactType = (int) DomainContactType.Billing;
          }
          else if (contactTypeValue.Equals("technical", StringComparison.OrdinalIgnoreCase))
          {
            contactType = (int)DomainContactType.Technical;
          }
          else if (contactTypeValue.Equals("administrative", StringComparison.OrdinalIgnoreCase))
          {
            contactType = (int)DomainContactType.Administrative;
          }
          else
          {
            contactType = (int)DomainContactType.Registrant;
          }
          ResponseAttributes = contactNode.Attributes;

          XmlNodeList xnlErrors = xdDoc.SelectNodes("/contact/error");
          if (xnlErrors != null)
          {
            foreach (XmlElement xlError in xnlErrors)
            {
              string sAttribute = xlError.GetAttribute("attribute");
              int iCode;
              int.TryParse(xlError.GetAttribute("code"), out iCode);
              string sDescription = xlError.GetAttribute("desc");
              string sDisplayString = xlError.GetAttribute("displaystring");
              string sDotType = xlError.GetAttribute("tld");

              Errors.Add(new DomainContactValidationError(sAttribute, iCode, sDescription, sDisplayString, sDotType, contactType));
            }
          }
        }

        XmlNodeList trusteeNodes = xdDoc.SelectNodes("/contact/trustee");
        if (trusteeNodes != null)
        {
          foreach (XmlElement element in trusteeNodes)
          {
            string dotType = element.GetAttribute("tld");
            string vendorId = element.GetAttribute("vendorid");
            TrusteeVendorIds[dotType] = vendorId;
          }
        }
      }
    }

    #region IResponseData Members

    private readonly AtlantisException _ex;
    public AtlantisException GetException()
    {
      return _ex;
    }

    private readonly string _responseXml;
    public string ToXML()
    {
      return _responseXml;
    }

    #endregion
  }
}
