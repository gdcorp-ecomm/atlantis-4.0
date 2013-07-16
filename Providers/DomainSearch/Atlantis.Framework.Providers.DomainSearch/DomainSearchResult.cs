using System.Collections.Generic;
using Atlantis.Framework.DomainSearch.Interface;
using Atlantis.Framework.Providers.DomainSearch.Interface;

namespace Atlantis.Framework.Providers.DomainSearch
{
  public class DomainSearchResult : IDomainSearchResult
  {
    private readonly string _jsonResponse;
    private Dictionary<string, IEnumerable<IFindResponseDomain>> _searchResultDomains;

    public DomainSearchResult(Dictionary<string, IEnumerable<IFindResponseDomain>> searchResultDomains, string jsonResponse)
    {
      _jsonResponse = jsonResponse;
      _searchResultDomains = searchResultDomains;
    }

    public Dictionary<string, IEnumerable<IFindResponseDomain>> AllDomains
    {
      get
      {
        if (_searchResultDomains == null)
        {
          _searchResultDomains = new Dictionary<string, IEnumerable<IFindResponseDomain>>();
        }

        return _searchResultDomains;
      }
    }

    public bool GetDomains(string domainGroupType, out IEnumerable<IFindResponseDomain> domains)
    {
      var success = AllDomains.TryGetValue(domainGroupType, out domains);

      return success;
    }


    public string ToJson()
    {
      return _jsonResponse ?? string.Empty;
    }
  }
}
