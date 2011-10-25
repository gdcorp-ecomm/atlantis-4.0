using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCAddDomain.Interface
{
  public class HCCAddDomainRequestData : RequestData
  {

    static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public HCCAddDomainRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string accountUid,
                                  string domainType,
                                  string parentDomainName,
                                  string domainToAdd,
                                  string directory, 
                                  bool enablePreviewDns,
                                  bool pathIsSubdirectoryName)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      DomainType = domainType;
      AccountUid = accountUid;
      ParentDomainName = parentDomainName;
      DomainToAdd = domainToAdd;
      Directory = directory;
      EnablePreviewDns = enablePreviewDns;
      PathIsSubdirectoryName = pathIsSubdirectoryName;
      RequestTimeout = _requestTimeout;
    }

    public string AccountUid { get; set; }
    public string DomainType { get; set; }
    public string ParentDomainName { get; set; }
    public string DomainToAdd { get; set; }
    public string Directory { get; set; }
    public bool EnablePreviewDns { get; set; }
    public bool PathIsSubdirectoryName { get; set; }
    
    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in HCCEditDomainRequestData");
    }


  }
}
