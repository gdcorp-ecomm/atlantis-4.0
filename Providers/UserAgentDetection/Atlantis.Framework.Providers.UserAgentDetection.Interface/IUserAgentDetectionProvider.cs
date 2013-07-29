
namespace Atlantis.Framework.Providers.UserAgentDetection.Interface
{
  public interface IUserAgentDetectionProvider
  {
    bool IsMobileDevice(string userAgent);

    bool IsNoRedirectBrowser(string userAgent);

    bool IsOutDatedBrowser(string userAgent);

    bool IsSearchEngineBot(string userAgent);
  }
}
