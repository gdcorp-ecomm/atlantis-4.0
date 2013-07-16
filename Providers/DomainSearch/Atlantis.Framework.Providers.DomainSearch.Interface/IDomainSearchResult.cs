using System.Collections.Generic;
using Atlantis.Framework.DomainSearch.Interface;

namespace Atlantis.Framework.Providers.DomainSearch.Interface
{
  public interface IDomainSearchResult
  {
    bool IsSuccess { get; }
    Dictionary<string, IList<IFindResponseDomain>> FindResponseDomains { get; }
    IList<IFindResponseDomain> GetDomainsByGroup(string domainGroupType);
    string JsonResponse { get;set; }
  }
}
