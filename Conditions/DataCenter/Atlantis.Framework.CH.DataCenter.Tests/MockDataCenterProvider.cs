using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DataCenter.Interface;
using Atlantis.Framework.Providers.Geo.Interface;

namespace Atlantis.Framework.CH.DataCenter.Tests
{
  public class MockDataCenterProvider : ProviderBase, IDataCenterProvider
  {
    private readonly Lazy<IGeoProvider> _geoProvider;

    public MockDataCenterProvider(IProviderContainer container) : base(container)
    {
      _geoProvider = new Lazy<IGeoProvider>(container.Resolve<IGeoProvider>);
    }

    public string DataCenterCode
    {
      get
      {
        string dataCenterCode;

        if (_geoProvider.Value.IsUserInCountry("JP"))
        {
          dataCenterCode = "AP";
        }
        else if (_geoProvider.Value.IsUserInCountry("IT"))
        {
          dataCenterCode = "EU";
        }
        else
        {
          dataCenterCode = "US";
        }

        return dataCenterCode;
      }
    }
  }
}
