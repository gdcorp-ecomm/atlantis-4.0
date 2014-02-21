using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.LogDomainSearchResults.Interface;
using Atlantis.Framework.Providers.LogDomainSearchResults.Interface;

namespace Atlantis.Framework.Providers.LogDomainSearchResults
{
  public class LogDomainSearchResultsProvider : ProviderBase, ILogDomainSearchResultsProvider
  {
    private readonly Lazy<ISiteContext> _siteContext;

    public LogDomainSearchResultsProvider(IProviderContainer container) : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => (container != null && container.CanResolve<ISiteContext>()) ? container.Resolve<ISiteContext>() : null);
    }

    public void SubmitLog(string domain, int availability)
    {
      if (string.IsNullOrEmpty(domain)) return;

      LogDomainSearchResultsRequestData request = new LogDomainSearchResultsRequestData(string.Empty, string.Empty, string.Empty, _siteContext.Value.Pathway, _siteContext.Value.PageCount, domain, availability)
      {
        RequestTimeout = TimeSpan.FromMilliseconds(50)
      };

      try
      {
        Engine.Engine.ProcessRequest(request, LoggingEngineRequests.LogDomainSearchResults);
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException("LogDomainSearchResultsProvider.SubmitLog", 0, message, request.ToXML());
        Engine.Engine.LogAtlantisException(aex);
      }
    }
  }
}