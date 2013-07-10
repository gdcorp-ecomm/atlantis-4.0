using System.Collections.Generic;
using Atlantis.Framework.DomainSearch.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Providers.DomainSearch.Interface
{
  public interface IDomainSearchProvider
  {
    /// <summary>
    /// Returns a list of domains from various domains group by Atlantis.Framework.Providers.DomainSearch.DomainDatabases
    /// </summary>
    /// <param name="searchPhrase">A comma delimited list of search phrases that may have spaces.</param>
    /// <param name="sourceCode"></param>
    /// <param name="sourceUrl"></param>
    /// <param name="domainResult"></param>
    bool SearchDomain(string searchPhrase, string sourceCode, string sourceUrl, out Dictionary<string, IEnumerable<IFindResponseDomain>> domainResult);
  }
}
