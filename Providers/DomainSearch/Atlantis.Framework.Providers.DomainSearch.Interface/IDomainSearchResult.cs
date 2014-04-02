using System.Collections.Generic;
using Atlantis.Framework.Domains.Interface;

namespace Atlantis.Framework.Providers.DomainSearch.Interface
{
  public interface IDomainSearchResult
  {
    bool IsSuccess { get; }
    Dictionary<string, IList<IFindResponseDomain>> FindResponseDomains { get; }
    IList<IFindResponseDomain> GetDomainsByGroup(string domainGroupType);
    string JsonRequest { get; set; }
    string JsonResponse { get; set; }
  }
}
