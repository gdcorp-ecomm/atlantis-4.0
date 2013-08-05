﻿using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Geo
{
  internal class GeoCountryData
  {
    IProviderContainer _container;
    List<IGeoCountry> _geoCountries;
    Dictionary<string, IGeoCountry> _geoCountriesByCode;

    internal GeoCountryData(IProviderContainer container)
    {
      _container = container;

      _geoCountries = new List<IGeoCountry>();
      _geoCountriesByCode = new Dictionary<string, IGeoCountry>(StringComparer.OrdinalIgnoreCase);

      CountryRequestData request = new CountryRequestData();
      CountryResponseData countries = null;

      try
      {
        countries = (CountryResponseData)DataCache.DataCache.GetProcessRequest(request, GeoProviderEngineRequests.Countries);
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException(request, "GeoCountryData.ctor", ex.Message + ex.StackTrace, string.Empty);
        Engine.Engine.LogAtlantisException(aex);
      }

      if (countries != null)
      {
        foreach (var country in countries.Countries)
        {
          IGeoCountry geoCountry = GeoCountry.FromCountry(_container, country);
          _geoCountries.Add(geoCountry);
          _geoCountriesByCode[geoCountry.Code] = geoCountry;
        }
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