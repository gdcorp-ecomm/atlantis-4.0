using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.Providers.Geo.Interface;

namespace Atlantis.Framework.Providers.DataCenter.Tests
{
  public class MockGeoProvider : ProviderBase, IGeoProvider
  {
    public MockGeoProvider(IProviderContainer container)
      : base(container)
    {
    }

    public string RequestCountryCode
    {
      get
      {
        string result = Container.GetData("MockGeoProvider.RequestCountryCode", "us");
        if (result == "error")
        {
          throw new Exception("error!");
        }
        return result;
      }
    }

    public bool IsUserInCountry(string countryCode)
    {
      return RequestCountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsUserInRegion(int regionTypeId, string regionName)
    {
      throw new NotImplementedException();
    }

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
  }
}
