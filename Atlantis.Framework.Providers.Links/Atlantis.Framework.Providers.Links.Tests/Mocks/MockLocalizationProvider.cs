using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Links.Tests.Mocks
{
  public class MockLocalizationProvider : ProviderBase, ILocalizationProvider
  {
    public MockLocalizationProvider(IProviderContainer container) : base(container)
    {
      
    }

    #region ILocalizationProvider Members

    public IEnumerable<IMarket> GetMarketsForCountryCode(string countryCode)
    {
      throw new NotImplementedException();
    }

    public string FullLanguage
    {
      get { throw new NotImplementedException(); }
    }

    public string ShortLanguage
    {
      get { throw new NotImplementedException(); }
    }

    private string _rewrittenUrlLanguage = String.Empty;
    public string RewrittenUrlLanguage
    {
      get { return _rewrittenUrlLanguage; }
      set { _rewrittenUrlLanguage = value; }
    }

    public bool IsActiveLanguage(string language)
    {
      throw new NotImplementedException();
    }

    public IMarket MarketInfo
    {
      get { throw new NotImplementedException(); }
    }

    public ICountrySite CountrySiteInfo
    {
      get { throw new NotImplementedException(); }
    }

    public string CountrySite
    {
      get { throw new NotImplementedException(); }
    }

    public bool IsGlobalSite()
    {
      return _IsGlobalSite;
    }

    private bool _IsGlobalSite;
    public void SetIsGlobalSite(bool b)
    {
      _IsGlobalSite = b;
    }

    public bool IsCountrySite(string countryCode)
    {
      throw new NotImplementedException();
    }

    public bool IsAnyCountrySite(HashSet<string> countryCodes)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<string> ValidCountrySiteSubdomains
    {
      get { throw new NotImplementedException(); }
    }

    public string GetCountrySiteLinkType(string baseLinkType)
    {
      throw new NotImplementedException();
    }

    public string PreviousCountrySiteCookieValue
    {
      get { throw new NotImplementedException(); }
    }

    public bool IsValidCountrySubdomain(string countryCode)
    {
      throw new NotImplementedException();
    }

    public void SetMarket(string marketId)
    {
      throw new NotImplementedException();
    }

    public System.Globalization.CultureInfo CurrentCultureInfo
    {
      get { throw new NotImplementedException(); }
    }

    public string GetLanguageUrl()
    {
      throw new NotImplementedException();
    }

    public string GetLanguageUrl(string marketId)
    {
      throw new NotImplementedException();
    }

    public string GetLanguageUrl(string countrySiteId, string marketId)
    {
      throw new NotImplementedException();
    }

    #endregion


    public ICountrySite TryGetCountrySite(string countrySiteId)
    {
      throw new NotImplementedException();
    }

    public IMarket TryGetMarket(string marketId)
    {
      throw new NotImplementedException();
    }

    public IMarket GetMarketForCountrySite(string countrySiteId, string marketId)
    {
      throw new NotImplementedException();
    }

    public IMarket TryGetMarketForCountrySite(string countrySiteId, string marketId)
    {
      throw new NotImplementedException();
    }

    public bool IsGlobalSite(string countrySiteId)
    {
      throw new NotImplementedException();
    }
  }
}
