using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.CMSIsDomainUsed.Interface
{
  public class CMSIsDomainUsedRequestData : RequestData
  {

    public List<string> DomainList { get; set; }

    public CMSIsDomainUsedRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  List<string> domainsToCheck)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(10);
      if (domainsToCheck == null)
      {
        DomainList = new List<string>();
      }
      else
      {
        DomainList = domainsToCheck;
      }
    }

    public override string ToXML()
    {
      //<ServiceRequest>
      //<DomainValid shopperId="856907">
      //<Domains>
      //<Domain>HowAboutThisOne.Com</Domain>
      //<Domain>WhateverGoesHere.Com</Domain>
      //</Domains>
      //</DomainValid>
      //</ServiceRequest>

      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("ServiceRequest");
      xtwRequest.WriteStartElement("DomainValid");
      xtwRequest.WriteAttributeString("shopperId", ShopperID);
      xtwRequest.WriteStartElement("Domains");
      foreach (string domainName in DomainList)
      {
        xtwRequest.WriteStartElement("Domain");
        xtwRequest.WriteString(domainName);
        xtwRequest.WriteEndElement();
      }
      xtwRequest.WriteEndElement();
      xtwRequest.WriteEndElement();
      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }


    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in CMSIsDomainUsedRequestData");     
    }


  }
}
