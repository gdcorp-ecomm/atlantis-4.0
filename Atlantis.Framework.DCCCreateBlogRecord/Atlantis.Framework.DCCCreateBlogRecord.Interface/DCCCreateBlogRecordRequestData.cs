using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCCreateBlogRecord.Interface
{
  public class DCCCreateBlogRecordRequestData : RequestData
  {
    public DCCCreateBlogRecordRequestData(string shopperId,
                                          string sourceUrl,
                                          string orderId,
                                          string pathway,
                                          int pageCount,
                                          int privateLabelId,
                                          string subDomainName,
                                          string domainName,
                                          string clientId,
                                          string endUserIp)

      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      SubDomainName = subDomainName;
      DomainName = domainName;
      ClientId = clientId;
      EndUserIp = endUserIp;
    }

    public string ClientId { get; protected set; }

    public string SubDomainName { get; protected set; }

    public string DomainName { get; protected set; }

    public string Origin
    {
      get { return "Customer"; }
    }

    public int PrivateLabelId { get; protected set; }

    public string EndUserIp { get; protected set; }

    public override string GetCacheMD5()
    {
      throw new Exception("DCCCreateBlogRecord is not a cacheable request.");
    }
  }
}