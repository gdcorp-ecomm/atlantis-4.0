
namespace Atlantis.Framework.Providers.DomainSearch.Interface
{
  public interface IDomainSearchProvider
  {
    IDomainSearchResult SearchDomain(string searchPhrase, string sourceCode, string sourceUrl);
  }
}
