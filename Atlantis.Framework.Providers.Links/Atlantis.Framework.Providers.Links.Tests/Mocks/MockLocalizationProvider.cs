using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Links.Tests.Mocks
{
  public class MockLocalizationProvider : ProviderBase, ILocalizationProvider
  {
    public MockLocalizationProvider(IProviderContainer container) : base(container)
    {
      
    }

    #region ILocalizationProvider Members

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
      throw new NotImplementedException();
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

    #endregion
  }
}
