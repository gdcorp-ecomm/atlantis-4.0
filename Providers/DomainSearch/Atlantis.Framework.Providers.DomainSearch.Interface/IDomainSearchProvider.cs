
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DomainSearch.Interface
{
  public interface IDomainSearchProvider
  {
    IDomainSearchResult SearchDomain(string searchPhrase, string sourceCode, string sourceUrl, ISplitTestInfo splitTestInfo = null);
    IDomainSearchResult SearchDomain(string searchPhrase, string sourceCode, string sourceUrl, IList<string> tldsToSearch, ISplitTestInfo splitTestInfo = null);
  }
}
