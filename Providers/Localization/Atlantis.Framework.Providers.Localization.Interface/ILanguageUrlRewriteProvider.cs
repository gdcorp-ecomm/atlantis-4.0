
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
    /// Get language from URL path and corresponding Market ID or saved info
    /// </summary>
    /// <param name="language">Any valid Language code from URL path or saved info</param>
    /// <param name="validMarketId">Corresponding Market ID for language found in URL path or saved info.  Or valid Market ID if one is in the URL path instead of a language code.</param>
    /// <returns>True if a valid language code was found in the URL path or saved info.  False otherwise.</returns>
    bool TryGetUrlLanguageAndMarketId(out string language, out string validMarketId);

    /// <summary>
    /// Get saved language from prior request process
    /// </summary>
    /// <param name="language">Saved language code</param>
    /// <param name="validMarketId">Saved market ID</param>
    /// <returns>True if saved language code was found.  False otherwise.</returns>
    bool TryGetSavedRequestLanguageUrlInfo(out string language, out string validMarketId);

    /// <summary>
    /// Returns whether or not the string passed in is a valid language code for the current Request CountrySite
    /// </summary>
    /// <param name="language">String to check</param>
    /// <returns></returns>
    bool IsValidLanguageCodeForRequest(string language);
  }
}
