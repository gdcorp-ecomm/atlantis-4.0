using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Providers.Localization.Interface;
using System.Collections;

namespace Atlantis.Framework.Providers.Geo
{
  internal class GeoCountryData
  {
    readonly List<IGeoCountry> _geoCountries;
    readonly Dictionary<string, IGeoCountry> _geoCountriesByCode;

    internal GeoCountryData(IProviderContainer container)
    {
      _geoCountries = new List<IGeoCountry>();
      _geoCountriesByCode = new Dictionary<string, IGeoCountry>(StringComparer.OrdinalIgnoreCase);

      var request = new CountryRequestData();
      CountryResponseData countries = null;

      try
      {
        countries = (CountryResponseData)DataCache.DataCache.GetProcessRequest(request, GeoProviderEngineRequests.Countries);
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException(request, "GeoCountryData.ctor", ex.Message + ex.StackTrace, string.Empty);
        Engine.Engine.LogAtlantisException(aex);
      }

      if (countries != null)
      {
        foreach (var country in countries.Countries)
        {
          IGeoCountry geoCountry = GeoCountry.FromCountry(container, country);
          _geoCountries.Add(geoCountry);
          _geoCountriesByCode[geoCountry.Code] = geoCountry;
        }
      }

      bool isSorted = false;
      try
      {
        ILocalizationProvider localization;
        if (container.TryResolve<ILocalizationProvider>(out localization) && localization.CurrentCultureInfo != null)
        {
          _geoCountries = _geoCountries.OrderBy(c => c.Name, StringComparer.Create(localization.CurrentCultureInfo, true)).ToList();
          isSorted = true;
        }
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException(request, "GeoCountryData.ctor", ex.Message + ex.StackTrace, string.Empty);
        Engine.Engine.LogAtlantisException(aex);
      }

      if (!isSorted)
      {
        _geoCountries = _geoCountries.OrderBy(c => c.Name).ToList();
      }
      
    }

    internal bool TryGetGeoCountry(string countryCode, out IGeoCountry country)
    {
      return _geoCountriesByCode.TryGetValue(countryCode, out country);
    }

    internal IEnumerable<IGeoCountry> Countries
    {
      get { return _geoCountries; }
    }
  }
}
