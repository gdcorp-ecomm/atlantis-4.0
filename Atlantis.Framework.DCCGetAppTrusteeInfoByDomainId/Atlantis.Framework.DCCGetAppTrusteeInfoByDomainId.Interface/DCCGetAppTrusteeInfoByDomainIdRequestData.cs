using System;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace Atlantis.Framework.DCCGetAppTrusteeInfoByDomainId.Interface
{
  public class DCCGetAppTrusteeInfoByDomainIdRequestData : RequestData
  {
    public DCCGetAppTrusteeInfoByDomainIdRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public List<string> DomainIds { get; set; }
    public TimeSpan RequestTimeout { get; set; }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetAppTrusteeInfoByDomainId is not a cacheable request.");
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("getapplicationtrusteeinfobydomainid");
      xtwRequest.WriteElementString("username", "FOS");
      xtwRequest.WriteStartElement("domains");
      if (DomainIds != null)
      {
        for (int i = 0; i < Math.Min(10, DomainIds.Count); ++i)
        {
          xtwRequest.WriteStartElement("domain");
          xtwRequest.WriteAttributeString("domainid", DomainIds[i]);
          xtwRequest.WriteEndElement();
        }
      }
      xtwRequest.WriteEndElement();
      xtwRequest.WriteEndElement();

      return sbRequest.ToString();
    }

    #endregion
  }
}
