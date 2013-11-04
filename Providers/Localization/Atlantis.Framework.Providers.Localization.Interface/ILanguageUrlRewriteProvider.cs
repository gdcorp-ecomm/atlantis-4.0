
namespace Atlantis.Framework.Providers.Localization.Interface
{
  public interface ILanguageUrlRewriteProvider
  {
    /// <summary>
    /// Redirects or rewrites the request Url based on language code in the path and the current country site
    /// </summary>
    /// <returns></returns>
    void ProcessLanguageUrl();

    /// <summary>
    /// Get language from URL path and corresponding Market ID
    /// </summary>
    /// <param name="language">Any valid Language code from URL path</param>
    /// <param name="validMarketId">Corresponding Market ID for language found in URL path.  Or valid Market ID if one is in the URL path instead of a language code.</param>
    /// <returns>True if a valid language code was found in the URL path.  False otherwise.</returns>
    bool TryGetUrlLanguageAndMarketId(out string language, out string validMarketId);
  }
}
