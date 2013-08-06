
namespace Atlantis.Framework.Providers.Localization.Interface
{
  public interface ILanguageUrlRewriteProvider
  {
    /// <summary>
    /// Redirects or rewrites the request Url based on language code in the path and the current country site
    /// </summary>
    /// <returns></returns>
    void ProcessLanguageUrl();
  }
}
