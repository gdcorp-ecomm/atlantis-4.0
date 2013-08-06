using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Testing.MockProviders;

namespace Atlantis.Framework.Providers.Localization.Tests
{
  public class MockGeoProvider : MockProviderBase, IGeoProvider 
  {
    public const string REQUEST_COUNTRY_SETTING_NAME = "MockGeoProvider.RequestCountry";

    public MockGeoProvider(IProviderContainer container) : base(container)
    {
    }

    public string RequestCountryCode
    {
      get
      {
        var result = GetMockSetting(REQUEST_COUNTRY_SETTING_NAME) as string;
        return result ?? "us";
      }
    }

    public bool IsUserInCountry(string countryCode)
    {
      return countryCode.Equals(RequestCountryCode, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsUserInRegion(int regionTypeId, string regionName)
    {
      return false;
    }

    public void SpoofUserIPAddress(string spoofIpAddress)
    {
    }

    #region IGeoProvider Members


    public IGeoLocation RequestGeoLocation
    {
      get { throw new NotImplementedException(); }
    }

    public System.Collections.Generic.IEnumerable<IGeoCountry> Countries
    {
      get { throw new NotImplementedException(); }
    }

    public bool TryGetCountryByCode(string countryCode, out IGeoCountry country)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
