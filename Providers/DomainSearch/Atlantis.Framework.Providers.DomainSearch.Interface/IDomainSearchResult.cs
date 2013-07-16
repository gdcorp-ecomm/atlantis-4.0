using System.Collections.Generic;
using Atlantis.Framework.DomainSearch.Interface;

namespace Atlantis.Framework.Providers.DomainSearch.Interface
{
  public interface IDomainSearchResult
  {
    Dictionary<string, IEnumerable<IFindResponseDomain>> AllDomains { get; }
    bool GetDomains(string domainGroupType, out IEnumerable<IFindResponseDomain> domains);
    string ToJson();
  }
}
