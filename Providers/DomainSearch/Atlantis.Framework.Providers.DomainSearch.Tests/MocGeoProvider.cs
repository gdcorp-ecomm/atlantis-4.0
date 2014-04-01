using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;

namespace Atlantis.Framework.Providers.DomainSearch.Tests
{
  public class MocGeoProvider : ProviderBase, IGeoProvider
  {
    public const string REQUEST_COUNTRY_SETTING_NAME = "MockGeoProvider.RequestCountry";
    public MocGeoProvider(IProviderContainer container) : base(container)
    {
      
    }
    public bool IsUserInCountry(string countryCode)
    {
      return countryCode.Equals(RequestCountryCode, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsUserInRegion(int regionTypeId, string regionName)
    {
      throw new NotImplementedException();
    }

    public bool TryGetCountryByCode(string countryCode, out IGeoCountry country)
    {
      throw new NotImplementedException();
    }

    public bool TryGetRegion(int regionTypeId, string regionName, out IGeoRegion region)
    {
      throw new NotImplementedException();
    }

    public string RequestCountryCode
    {
      get
      {
        return Container.GetData(REQUEST_COUNTRY_SETTING_NAME, "us");
      }
    }

    public IGeoLocation RequestGeoLocation { get; private set; }
    public IEnumerable<IGeoCountry> Countries { get; private set; }
  }
}
