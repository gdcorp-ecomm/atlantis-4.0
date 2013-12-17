using System;
using System.Collections.Generic;
using System.Globalization;

namespace Atlantis.Framework.Providers.Localization.Interface
{
  /// <summary>
  /// Provider interface used to obtain language and country site context
  /// </summary>
  public interface ILocalizationProvider
  {
    /// <summary>
    /// Will return the full language in language-locale form if available
    /// Example: en-au, fr-ca.  
    /// </summary>
    string FullLanguage { get; }

    /// <summary>
    /// Will return the short language only without any -locale
    /// </summary>
    string ShortLanguage { get; }

    /// <summary>
    /// Returns the language code found and removed by the ILanguageUrlRewriteProvider instance.
    /// This can be used for downstream processing and link building.
    /// </summary>
    string RewrittenUrlLanguage { get; set; }

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
    /// Returns the ICountrySite object for the request.  Depending on which provider implementation you use this will be
    /// based on either the cookie value or the subdomain on the request.
    /// </summary>
    ICountrySite CountrySiteInfo { get; }

    /// <summary>
    /// Returns the country site of the request. Depending on which provider implementation you use this will be
    /// based on either the cookie value or the subdomain on the request.
    /// </summary>
    string CountrySite { get; }

    /// <summary>
    /// Returns the IMarket object that represents the Market for the request
    /// </summary>
    IMarket MarketInfo { get; }

    /// <summary>
    /// Get the ICountrySite for the countrySiteId
    /// </summary>
    /// <param name="countrySiteId"></param>
    /// <returns>valid interface or null if the countrySiteId is invalid</returns>
    ICountrySite TryGetCountrySite(string countrySiteId);

    /// <summary>
    /// Get the IMarket for the marketId.  The market need not have any relation to the current request's CountrySite.
    /// </summary>
    /// <param name="marketId"></param>
    /// <returns>valid interface or null if the marketId is invalid</returns>
    IMarket TryGetMarket(string marketId);

    /// <summary>
    /// Get the IMarket for the marketId for the given CountrySite.  
    /// If the countrySiteId is invalid, the CountrySite of the current request will be used in its place.
    /// If the market is not mapped to the CountrySite, then the default market of the current request's CountrySite will be returned (even if no mapping is in place).
    /// </summary>
    /// <param name="countrySiteId"></param>
    /// <param name="marketId"></param>
    /// <returns></returns>
    IMarket GetMarketForCountrySite(string countrySiteId, string marketId);

    /// <summary>
    /// Get the IMarket for the marketId for the given CountrySite.  
    /// </summary>
    /// <param name="countrySiteId"></param>
    /// <param name="marketId"></param>
    /// <returns>If the countrySiteId is invalid, or the marketId is invalid, or the market is not mapped to the CountrySite, null is returned.</returns>
    IMarket TryGetMarketForCountrySite(string countrySiteId, string marketId);
 
    /// <summary>
    /// Returns true if the request is on the global (non-country) site. This will return true if you are on the "es" site
    /// also because that is a language site, not a country site.
    /// </summary>
    /// <returns>true if not on a country site</returns>
    bool IsGlobalSite();

    /// <summary>
    /// Returns true if the countrySiteId is the global (non-country) site. 
    /// </summary>
    /// <returns>true if not on a country site</returns>
    bool IsGlobalSite(string countrySiteId);

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
    /// Returns the previous market id  preference value in the market id (language) cookie.
    /// </summary>
    string PreviousLanguageCookieValue { get; }

    /// <summary>
    /// Returns true if the given is valid country subdomain (not case sensitive).
    /// </summary>
    /// <param name="countryCode">Country code.</param>
    /// <returns>Returns true if the given is valid country subdomain (not case sensitive).</returns>
    bool IsValidCountrySubdomain(string countryCode);

    /// <summary>
    /// Sets the Market for the request
    /// </summary>
    /// <param name="marketId">Market ID to set as the Market for the request</param>
    void SetMarket(string marketId);

    /// <summary>
    /// Returns the CultureInfo of the current request.
    /// </summary>
    CultureInfo CurrentCultureInfo { get; }

    /// <summary>
    /// Gets the Language Url fragment for the current request
    /// </summary>
    /// <returns></returns>
    string GetLanguageUrl();

    /// <summary>
    /// Gets the Language Url fragment for the specific marketId, the countrySiteId will be for the current request
    /// </summary>
    /// <returns></returns>
    string GetLanguageUrl(string marketId);

    /// <summary>
    /// Gets the Language Url fragment for the specific marketId, and countrySiteId.
    /// </summary>
    /// <returns></returns>
    string GetLanguageUrl(string countrySiteId, string marketId);

    /// <summary>
    /// Gets the IMarket list for a given countrycode. If the country code is invalid returns the default www IMarket list.
    /// </summary>
    /// <returns>IMarket list for a given countrycode or returns default www</returns>    
    [Obsolete("Use GetMappedMarketsForCountrySite(string countrySite, bool includeInternalOnly)")]
    IEnumerable<IMarket> GetMarketsForCountryCode(string countryCode);

    /// <summary>
    /// Gets the IMarket list for a given CountrySite.  If the CountrySite is invalid then return empty collection or, if provided, mapped IMarkets for the specified default CountrySite.
    /// </summary>
    /// <param name="countrySite">CountrySite to get list of mapped IMarket</param>
    /// <param name="includeInternalOnly">Include IMarkets for internal-only Mappings, Markets, and CountrySites</param>
    /// <returns>IMarket list for a given countrycode</returns>        
    IEnumerable<IMarket> GetMappedMarketsForCountrySite(string countrySite, bool includeInternalOnly);

  }
}
