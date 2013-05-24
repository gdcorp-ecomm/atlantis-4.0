using System.Collections.Generic;
namespace Atlantis.Framework.Providers.Localization.Interface
{
  /// <summary>
  /// Provider interface used to obtain language and country site context
  /// </summary>
  public interface ILocalizationProvider
  {
    /// <summary>
    /// Will return the full language in language-dialect form if available
    /// Example: en-au, fr-ca.  
    /// </summary>
    string FullLanguage { get; }

    /// <summary>
    /// Will return the short language only without any -dialect
    /// </summary>
    string ShortLanguage { get; }

    /// <summary>
    /// Will evaluate the given language versus the language of the request.
    /// If you pass only a short language, it will return true for any dialect of that langauge
    /// If you pass a full language (like en-us) it will only return true if it is an exact match
    /// This comparison ignores case.
    /// </summary>
    /// <param name="language">short or full language to check</param>
    /// <returns>true if the request languages matches using the rules in the summary</returns>
    bool IsActiveLanguage(string language);

    /// <summary>
    /// Returns the country site of the request. Depending on which provider implementation you use this will be
    /// based on either the cookie value or the subdomain on the request.
    /// </summary>
    string CountrySite { get; }

    /// <summary>
    /// Returns true if the request is on the global (non-country) site. This will return www if you are on the "es" site
    /// also because that is a language site, not a country site.
    /// </summary>
    /// <returns>true if not on a country site</returns>
    bool IsGlobalSite();

    /// <summary>
    /// Returns true if the given countrycode matches the country site of the request.
    /// This check ignores case
    /// </summary>
    /// <param name="countryCode">country code to check</param>
    /// <returns>true if the request countrysite matches the given countryCode</returns>
    bool IsCountrySite(string countryCode);

    /// <summary>
    /// Returns true if any of the given countrycodes match the country site of the request.
    /// This check ignores case
    /// </summary>
    /// <param name="countryCodes">HashSet of country codes</param>
    /// <returns>true if any of the given countrycodes match the country site of the request.</returns>
    bool IsAnyCountrySite(HashSet<string> countryCodes);

    /// <summary>
    /// Returns an enumerable list of the valid country codes that can be used on a subdomain or as the countrysite cookie value.
    /// </summary>
    IEnumerable<string> ValidCountrySiteSubdomains { get; }

    /// <summary>
    /// Returns the linktype with a valid country extension on it in the form of ".XX".  If not on a country site,
    /// the linktype is returned unchanged.
    /// </summary>
    /// <param name="baseLinkType">linktype to adjust</param>
    /// <returns>countrysite specific linktype if on a country site</returns>
    string GetCountrySiteLinkType(string baseLinkType);

    /// <summary>
    /// Returns the previous country preference value in the countrysite cookie.
    /// </summary>
    string PreviousCountrySiteCookieValue { get; }

    /// <summary>
    /// Returns true if the given is valid country subdomain (not case sensitive).
    /// </summary>
    /// <param name="countryCode">Country code.</param>
    /// <returns>Returns true if the given is valid country subdomain (not case sensitive).</returns>
    bool IsValidCountrySubdomain(string countryCode);
  }
}
