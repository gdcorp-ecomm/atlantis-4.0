﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainContactValidation.Interface
{
  public class DomainContactValidationRequestData : RequestData
  {
    private readonly DomainContactValidation _domainContactValidation;
    private readonly string _domainCheckType;
    private readonly int _domainContactType;
    private readonly IEnumerable<string> _tlds;
    private readonly int _privateLabelId;
    private readonly string _marketId;
    private readonly IEnumerable<string> _domains;
      
    [Obsolete("Market ID needed for error translation. Use non-obsolete constructor.")]
    public DomainContactValidationRequestData(string checkType, int domainContactType,
                                              DomainContactValidation domainContact, IEnumerable<string> tlds, int privateLabelId)
    {
      _domainContactValidation = domainContact;
      _domainContactType = domainContactType;
      _domainCheckType = checkType;
      _tlds = tlds;
      _privateLabelId = privateLabelId;
      RequestTimeout = TimeSpan.FromSeconds(4d);
    }

    [Obsolete(".UK changes require the domains are also passed to the validation service")]
    public DomainContactValidationRequestData(string checkType, int domainContactType,
                                              DomainContactValidation domainContact, IEnumerable<string> tlds, int privateLabelId, string marketId)
    {
      _domainContactValidation = domainContact;
      _domainContactType = domainContactType;
      _domainCheckType = checkType;
      _tlds = tlds;
      _privateLabelId = privateLabelId;
      _marketId = marketId;
      RequestTimeout = TimeSpan.FromSeconds(4d);
    }

    public DomainContactValidationRequestData(string checkType, int domainContactType, DomainContactValidation domainContact, IEnumerable<string> tlds, 
                                              int privateLabelId, string marketId, IEnumerable<string> domains )
    {
      _domainContactValidation = domainContact;
      _domainContactType = domainContactType;
      _domainCheckType = checkType;
      _tlds = tlds;
      _privateLabelId = privateLabelId;
      _marketId = marketId;
      _domains = domains;
      RequestTimeout = TimeSpan.FromSeconds(4d);   
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("DomainContactValidation is not a cacheable request.");
    }

    /// <summary>
    ///  Generates an Xml string matching the following exemplar:
    ///  <?xml version="1.0" encoding="utf-8"	standalone="no"?>
    ///  <contact type="xfer"	contactType="0"  tlds="COM"
    ///           fname="John"	lname="Doe"  org="NimbleWits Inc"
    ///           sa1="101 N California"  sa2="Suite 201" 
    ///           city="Denver"  sp="CO"  pc="80126" cc="us"
    ///           phone="303-555-2222"	fax="303-555-1111"
    ///           email="jdoe@NimbleWits.com"
    ///           privateLabelId="1"/>
    /// </summary>
    /// <returns>A string containing required XML sequence</returns>
    public override string ToXML()
    {
      var sbRequest = new StringBuilder();
      var oXmlTextWriter = new XmlTextWriter(new StringWriter(sbRequest));

      oXmlTextWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-16\" standalone=\"no\"");

      oXmlTextWriter.WriteStartElement("contact");
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.Type, _domainCheckType == DomainCheckType.DOMAIN_TRANFER ? "xfer" : "");
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.DomainContactType, GetContactTypeString(_domainContactType));
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.Tlds, string.Join("|", _tlds));
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.FirstName, _domainContactValidation.FirstName);
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.LastName, _domainContactValidation.LastName);
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.Organization, _domainContactValidation.Company);
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.Address1, _domainContactValidation.Address1);
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.Address2, _domainContactValidation.Address2);
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.City, _domainContactValidation.City);
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.State, _domainContactValidation.State);
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.Zip, _domainContactValidation.Zip);
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.Country, _domainContactValidation.Country);
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.Phone, _domainContactValidation.Phone);
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.Fax, _domainContactValidation.Fax);
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.Email, _domainContactValidation.Email);
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.PrivateLabelId, _privateLabelId.ToString());
      oXmlTextWriter.WriteAttributeString(DomainContactAttributes.CanadianPresence, _domainContactValidation.CanadianPresence);
      if (!string.IsNullOrEmpty(_marketId))
      {
        oXmlTextWriter.WriteAttributeString(DomainContactAttributes.MarketId, _marketId);        
      }

      if (_domains != null && _domains.Any())
      {
        oXmlTextWriter.WriteStartElement(DomainContactAttributes.Domains);
        
        foreach (string domain in _domains)
        {
          oXmlTextWriter.WriteStartElement(DomainContactAttributes.Domain);
          oXmlTextWriter.WriteAttributeString(DomainContactAttributes.DomainName, domain);
          oXmlTextWriter.WriteEndElement();
        }
        oXmlTextWriter.WriteEndElement();
      }

      oXmlTextWriter.WriteEndElement();

      return sbRequest.ToString();
    }

    private static string GetContactTypeString(int contactType)
    {
      string result = string.Empty;
      switch (contactType)
      {
        case DomainContactType.REGISTRANT:
          result = "registrant";
          break;
        case DomainContactType.TECHNICAL:
          result = "technical";
          break;
        case DomainContactType.ADMINISTRATIVE:
          result = "administrative";
          break;
        case DomainContactType.BILLING:
          result = "billing";
          break;
      }
      return result;
    }

    #endregion

  }
}
