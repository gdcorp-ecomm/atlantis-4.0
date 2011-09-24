using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCRemoveDomain.Interface
{
  public class HCCRemoveDomainRequestData : RequestData
  {

    static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public HCCRemoveDomainRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string accountUid,
                                  string domainType,
                                  string parentDomainName,
                                  string domainToDelete)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      DomainType = domainType;
      AccountUid = accountUid;
      ParentDomainName = parentDomainName;
      DomainToDelete = domainToDelete;
      RequestTimeout = _requestTimeout;
    }

    public string AccountUid { get; set; }
    public string DomainType { get; set; }
    public string ParentDomainName { get; set; }
    public string DomainToDelete { get; set; }
    
    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in HCCEditDomainRequestData");
    }
  }
}
