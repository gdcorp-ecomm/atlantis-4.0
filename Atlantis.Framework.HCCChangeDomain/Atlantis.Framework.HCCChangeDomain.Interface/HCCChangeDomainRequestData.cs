using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCChangeDomain.Interface
{
  public class HCCChangeDomainRequestData : RequestData
  {
    static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public HCCChangeDomainRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string accountUid,
                                  string domainUid,
                                  string newDomainName,
                                  bool previewDns,
                                  string newDomanPath)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = _requestTimeout;
      AccountUid = accountUid;
      DomainUid = domainUid;
      NewDomainName = newDomainName;
      PreviewDns = previewDns;
      NewDomainPath = newDomanPath;
    }

    public string AccountUid { get; set; }
    public string DomainUid { get; set; }
    public string NewDomainName { get; set; }
    public bool PreviewDns { get; set; }
    public string NewDomainPath { get; set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in HCCChangeDomainRequestData");     
    }


  }
}
