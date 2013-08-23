using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.DomainSearch.Tests
{
    public class LocalizationProviderTestProxy : ProviderBase, ILocalizationProvider // Framework providers should implement a corresponding interface
    {
        public LocalizationProviderTestProxy(IProviderContainer container)
            : base(container)
        {
        }


        public string FullLanguage
        {
            get { return "en-us"; }
        }

        public string ShortLanguage
        {
            get { return "en"; }
        }

        public bool IsActiveLanguage(string language)
        {
            return language == "en";
        }

        public string CountrySite
        {
            get { return "www"; }
        }

        public bool IsGlobalSite()
        {
            return true;
        }

        public bool IsCountrySite(string countryCode)
        {
            return countryCode.ToLowerInvariant() != "us";
        }

        public bool IsAnyCountrySite(System.Collections.Generic.HashSet<string> countryCodes)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<string> ValidCountrySiteSubdomains
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

        public void SetLanguage(string language)
        {
            throw new NotImplementedException();
        }

        public System.Globalization.CultureInfo CurrentCultureInfo
        {
            get { throw new NotImplementedException(); }
        }


        public string RewrittenUrlLanguage
        {
          get
          {
            throw new NotImplementedException();
          }
          set
          {
            throw new NotImplementedException();
          }
        }

        public ICountrySite CountrySiteInfo
        {
          get { throw new NotImplementedException(); }
        }

        public IMarket MarketInfo
        {
          get { throw new NotImplementedException(); }
        }

        public void SetMarket(string marketId)
        {
          throw new NotImplementedException();
        }
    }
}
