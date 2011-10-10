using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetForwarding.Interface
{
  public class DCCGetForwardingRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(6);

    public DCCGetForwardingRequestData( string shopperId,
                                        string sourceUrl,
                                        string orderId,
                                        string pathway,
                                        int pageCount,
                                        string domainName)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      DomainName = domainName;
      RequestTimeout = _requestTimeout;
    }

    public string DomainName { get; set; }

    public override string GetCacheMD5()
    {
      throw new Exception("DCCGetForwarding is not a cacheable request.");
    }
  }
}
