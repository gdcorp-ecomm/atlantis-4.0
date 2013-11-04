namespace Atlantis.Framework.Providers.Localization.Interface
{
  /// <summary>
  /// Provider interface used to obtain language and country site context
  /// </summary>
  public interface ILocalizationRedirectResponse
  {
    /// <summary>
    /// Return whether a country site should redirect.
    /// </summary>
    bool ShouldRedirect { get; }

    /// <summary>
    /// Will return the short language only without any -dialect the translation to use on the url
    /// Example: ES, EN etc
    /// </summary>
    string ShortLanguage { get; set; }

    /// <summary>
    /// Returns the country site  to redirect.
    /// Example: EU, CA etc.  
    /// </summary>
    string CountrySite { get; set; }

    /// <summary>
    /// Returns the market ID for redirect purposes
    /// Example: en-US, es-US, en-CA, fr-CA, pt-BR, etc.  
    /// </summary>
    string MarketId { get; set; }
  }
}
