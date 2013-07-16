
namespace Atlantis.Framework.Providers.DomainSearch.Interface
{
  public interface IDomainSearchProvider
  {
    bool SearchDomain(string searchPhrase, string sourceCode, string sourceUrl, out IDomainSearchResult domainSearchResult);
  }
}
