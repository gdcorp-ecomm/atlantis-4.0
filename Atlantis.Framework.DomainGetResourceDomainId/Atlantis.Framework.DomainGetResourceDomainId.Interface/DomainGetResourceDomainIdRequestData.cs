using System;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainGetResourceDomainId.Interface
{
  public class DomainGetResourceDomainIdRequestData : RequestData
  {
    public string Domain { get; private set; }
    public string DomainOrderId { get; private set; }
    public string DomainOwnerShopperId { get; private set; }

    public DomainGetResourceDomainIdRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string domain, string domainOrderId, string domainOwnerShopperId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      Domain = domain;
      DomainOrderId = domainOrderId;
      DomainOwnerShopperId = domainOwnerShopperId;

      this.RequestTimeout = TimeSpan.FromMilliseconds(5000);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

  }
}
