using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Geo
{
  public class GeoCountry : IGeoCountry
  {
    internal static IGeoCountry FromCountry(IProviderContainer container, Country country)
    {
      return new GeoCountry(container, country);
    }

    private Lazy<GeoStateData> _geoStateData;
    private IProviderContainer _container;
    private Country _country;

    private GeoCountry(IProviderContainer container, Country country)
    {
      _container = container;
      _country = country;
      _geoStateData = new Lazy<GeoStateData>(() => { return new GeoStateData(_container, Id); });
    }

    public int Id
    {
      get { return _country.Id; }
    }

    public string Code
    {
      get { return _country.Code; }
    }

    public string Name
    {
      get
      { 
        return _country.Name;
      }
    }

    public string CallingCode
    {
      get { return _country.CallingCode; }
    }

    public bool HasStates
    {
      get { return _geoStateData.Value.HasStates; }
    }

    public IEnumerable<IGeoState> States
    {
      get { return _geoStateData.Value.States; }
    }

    public bool TryGetStateByCode(string stateCode, out IGeoState state)
    {
      return _geoStateData.Value.TryGetGeoStateByCode(stateCode, out state);
    }
  }
}
