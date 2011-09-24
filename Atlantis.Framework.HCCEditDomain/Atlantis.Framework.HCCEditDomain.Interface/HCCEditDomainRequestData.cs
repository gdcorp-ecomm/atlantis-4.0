using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCEditDomain.Interface
{
  public class HCCEditDomainRequestData : RequestData
  {
    static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);
    

    public HCCEditDomainRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string accountUid,
                                  string domainType,
                                  string parentDomainName,
                                  string newSubDomainName,
                                  string domainToEdit,
                                  string newPath)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      DomainType = domainType;
      AccountUid = accountUid;
      ParentDomainName = parentDomainName;
      NewSubDomainName = newSubDomainName;
      DomainToEdit = domainToEdit;
      NewPath = newPath;
      RequestTimeout = _requestTimeout;
    }

    public string AccountUid { get; set; }
    public string DomainType { get; set; }
    public string ParentDomainName { get; set; }
    public string NewSubDomainName { get; set; }
    public string DomainToEdit { get; set; }
    public string NewPath { get; set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in HCCEditDomainRequestData");     
    }


  }
}
