namespace Atlantis.Framework.Providers.LogDomainSearchResults.Interface
{
  public interface ILogDomainSearchResultsProvider
  {
    void SubmitLog(string domain, int availability);
  }
}