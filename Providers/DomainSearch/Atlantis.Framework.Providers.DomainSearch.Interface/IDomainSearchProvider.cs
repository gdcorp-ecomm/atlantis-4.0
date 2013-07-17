
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DomainSearch.Interface
{
  public interface IDomainSearchProvider
  {
    IDomainSearchResult SearchDomain(string searchPhrase, string sourceCode, string sourceUrl);
    IDomainSearchResult SearchDomain(string searchPhrase, string sourceCode, string sourceUrl, IList<string> tldsToSearch);
  }
}
