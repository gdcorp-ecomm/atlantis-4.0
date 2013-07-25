using System.Collections.Generic;
using Atlantis.Framework.DomainSearch.Interface;
using Atlantis.Framework.Providers.DomainSearch.Interface;

namespace Atlantis.Framework.Providers.DomainSearch
{
  public class DomainSearchResult : IDomainSearchResult
  {
    private static readonly IList<IFindResponseDomain> _emptyDomains = new List<IFindResponseDomain>(0); 

    private Dictionary<string, IList<IFindResponseDomain>> _searchResultDomains;

    public DomainSearchResult(bool isSuccess, Dictionary<string, IList<IFindResponseDomain>> searchResultDomains)
    {
      _isSuccess = isSuccess;
      _searchResultDomains = searchResultDomains;
    }

    public Dictionary<string, IList<IFindResponseDomain>> FindResponseDomains
    {
      get
      {
        if (_searchResultDomains == null)
        {
          _searchResultDomains = new Dictionary<string, IList<IFindResponseDomain>>();
        }

        return _searchResultDomains;
      }
    }

    public IList<IFindResponseDomain> GetDomainsByGroup(string domainGroupType)
    {
      IList<IFindResponseDomain> domains;
      if (!FindResponseDomains.TryGetValue(domainGroupType, out domains))
      {
        domains = _emptyDomains;
      }

      return domains;
    }

    private string _jsonResponse;
    public string JsonResponse 
    {
      get { return _jsonResponse ?? string.Empty; }
      set { _jsonResponse = value; }
    }

    
    private readonly bool _isSuccess;
    public bool IsSuccess
    {
      get { return _isSuccess; }
    }
  }
}
