using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using System;
using System.Collections.Generic;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Geo
{
  public class GeoCountry : IGeoCountry
  {
    internal static IGeoCountry FromCountry(IProviderContainer container, Country country)
    {
      return new GeoCountry(container, country);
    }

    private readonly Lazy<GeoStateData> _geoStateData;
    private readonly IProviderContainer _container;
    private readonly Country _country;
    private string _name;

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
        return _name ?? (_name = DetermineLanguageAwareName());
      }
    }

    private string DetermineLanguageAwareName()
    {
      var result = _country.Name;

      if (!_container.CanResolve<ILocalizationProvider>())
      {
        return result;
      }

      var localization = _container.Resolve<ILocalizationProvider>();
      if (localization.IsActiveLanguage("en-us"))
      {
        return result;
      }

      try
      {
        var request = new CountryNamesRequestData(localization.FullLanguage);
        var response = (CountryNamesResponseData)DataCache.DataCache.GetProcessRequest(request, GeoProviderEngineRequests.CountryNames);
        string languageName;
        if (response.TryGetNameById(_country.Id, out languageName))
        {
          result = languageName;
        }
      }
      catch
      {
        // DataCache logged the exception, keep default english name.
      }

      return result;
    }

    public string CallingCode
    {
      get
      {
        return _country.CallingCode;
      }
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
