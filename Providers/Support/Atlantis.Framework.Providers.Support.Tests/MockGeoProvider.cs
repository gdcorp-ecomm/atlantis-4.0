using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;

namespace Atlantis.Framework.Providers.Support.Tests
{
  public class MockGeoProvider : ProviderBase, IGeoProvider
  {
    public const string REQUEST_COUNTRY_SETTING_NAME = "MockGeoProvider.RequestCountry";

    public MockGeoProvider(IProviderContainer container) : base(container)
    {
    }

    public string RequestCountryCode 
    {
      get
      {
        return Container.GetData(REQUEST_COUNTRY_SETTING_NAME, "us");
      }
    }

    public bool IsUserInCountry(string countryCode)
    {
      throw new NotImplementedException();
    }

    public bool IsUserInRegion(int regionTypeId, string regionName)
    {
      throw new NotImplementedException();
    }

    public IGeoLocation RequestGeoLocation { get; private set; }
    public IEnumerable<IGeoCountry> Countries { get; private set; }
    public bool TryGetCountryByCode(string countryCode, out IGeoCountry country)
    {
      throw new NotImplementedException();
    }
  }
}
