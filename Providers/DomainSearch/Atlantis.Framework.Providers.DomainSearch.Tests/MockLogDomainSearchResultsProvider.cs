using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.Providers.Logging.Interface;

namespace Atlantis.Framework.Providers.DomainSearch.Tests
{
  public class MockLogDomainSearchResultsProvider : ProviderBase, ILogDomainSearchResultsProvider
  {
    public MockLogDomainSearchResultsProvider(IProviderContainer container)
      : base(container)
    {
    }

    public void SubmitLog(string domain, int availability)
    {
    }
  }
}
