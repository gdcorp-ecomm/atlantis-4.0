using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;

namespace Atlantis.Framework.Providers.Products.Tests
{
  public class MockGeoProvider : ProviderBase, IGeoProvider // Framework providers should implement a corresponding interface
  {
    public MockGeoProvider(IProviderContainer container)
      : base(container)
    {
    }

    public IEnumerable<IGeoCountry> Countries
    {
      get { throw new NotImplementedException(); }
    }

    public bool IsUserInCountry(string countryCode)
    {
      throw new NotImplementedException();
    }

    public bool IsUserInRegion(int regionTypeId, string regionName)
    {
      throw new NotImplementedException();
    }

    public string RequestCountryCode
    {
      get { return Container.GetData("MockGeoProvider.CountryCode", "us"); }
    }

    public IGeoLocation RequestGeoLocation
    {
      get { throw new NotImplementedException(); }
    }

    public bool TryGetCountryByCode(string countryCode, out IGeoCountry country)
    {
      throw new NotImplementedException();
    }
  }
}
