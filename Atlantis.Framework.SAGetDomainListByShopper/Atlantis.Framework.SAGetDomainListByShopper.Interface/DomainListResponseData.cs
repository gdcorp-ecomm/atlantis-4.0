using System.Collections.Generic;
using Atlantis.Framework.SA.Interface;

namespace Atlantis.Framework.SAGetDomainListByShopper.Interface
{
  public class DomainListResponseData : SAResponseBase
  {
    private List<string> _domainList = new List<string>();

    public string DomainCount { get; set; }
    public List<string> DomainList
    {
      get { return _domainList ?? (_domainList = new List<string>()); }
      set { _domainList = value; }
    }
  }
}
