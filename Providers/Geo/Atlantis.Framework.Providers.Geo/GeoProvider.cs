using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using System;
using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Providers.Geo
{
  public class GeoProvider : ProviderBase, IGeoProvider
  {
    Lazy<ISiteContext> _siteContext;
    Lazy<IProxyContext> _proxyContext;
    Lazy<string> _requestCountryCode;
    Lazy<string> _ipAddress;
    Lazy<GeoCountryData> _geoCountryData;

    IGeoLocation _requestLocation = null;

    public GeoProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => { return Container.Resolve<ISiteContext>(); });
      _proxyContext = new Lazy<IProxyContext>(() => { return LoadProxyContext(); });
      _requestCountryCode = new Lazy<string>(() => { return DetermineRequestCountryCode(); });
      _ipAddress = new Lazy<string>(() => { return GetRequestIP(); });
      _geoCountryData = new Lazy<GeoCountryData>(() => { return new GeoCountryData(Container); });
    }

    private IProxyContext LoadProxyContext()
    {
      IProxyContext result;
      if (!Container.TryResolve<IProxyContext>(out result))
      {
        result = null;
      }
      return result;
    }

    private string DetermineRequestCountryCode()
    {
      string result = "us";
      if (!string.IsNullOrEmpty(_ipAddress.Value))
      {
        string alreadyLoadedCountryCode;
        if (TryGetCountryCodeFromLocation(out alreadyLoadedCountryCode))
        {
          result = alreadyLoadedCountryCode;
        }
        else
        {
          result = LookupCountryByIP(_ipAddress.Value);      
        }
      }

      return result;
    }

    private string LookupCountryByIP(string ipAddress)
    {
      string result = "us";

      try
      {
        var request = new IPCountryLookupRequestData(ipAddress);
        var response = (IPCountryLookupResponseData)Engine.Engine.ProcessRequest(request, GeoProviderEngineRequests.IPCountryLookup);

        if (response.CountryFound)
        {
          result = response.CountryCode;
        }
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException("GeoProvider.LookupCountryByIP", 0, ex.Message, ex.StackTrace);
        Engine.Engine.LogAtlantisException(exception);
      }

      return result;
    }

    private bool TryGetCountryCodeFromLocation(out string countryCode)
    {
      countryCode = null;
      bool result = false;

      if ((_requestLocation != null) && (!string.IsNullOrEmpty(_requestLocation.CountryCode)))
      {
        countryCode = _requestLocation.CountryCode;
        result = true;
      }

      return result;
    }

    private string GetSpoofIp()
    {
      var result = string.Empty;
      if (_siteContext.Value.IsRequestInternal && HttpContext.Current != null)
      {
        var spoofIp = HttpContext.Current.Request.QueryString["qaspoofip"];

        if (!string.IsNullOrEmpty(spoofIp))
        {
          result = spoofIp;
        }
      }

      return result;
    }

    private string GetRequestIP()
    {
      string result = GetSpoofIp();

      if (string.IsNullOrEmpty(result))
      {
        if (_proxyContext.Value != null)
        {
          result = _proxyContext.Value.OriginIP;
        }
        else if (HttpContext.Current != null)
        {
          result = HttpContext.Current.Request.UserHostAddress;
        }
      }

      return result;
    }

    private CountryResponseData LoadCountries()
    {
      var request = new CountryRequestData();
      return (CountryResponseData)DataCache.DataCache.GetProcessRequest(request, GeoProviderEngineRequests.Countries);
    }

    public string RequestCountryCode
    {
      get { return _requestCountryCode.Value; }
    }

    public bool IsUserInCountry(string countryCode)
    {
      if (string.IsNullOrEmpty(countryCode))
      {
        return false;
      }

      return  countryCode.Equals(_requestCountryCode.Value, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsUserInRegion(int regionTypeId, string regionName)
    {
      bool result = false;

      try
      {
        IGeoCountry country;
        if (_geoCountryData.Value.TryGetGeoCountry(_requestCountryCode.Value, out country))
        {
          var regionRequest = new RegionRequestData(regionTypeId, regionName);
          var regionResponse = (RegionResponseData)DataCache.DataCache.GetProcessRequest(regionRequest, GeoProviderEngineRequests.Regions);
          result = regionResponse.HasCountry(country.Id);
        }
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException("GeoProvider.IsUserInRegion", 0, ex.Message, ex.StackTrace);
        Engine.Engine.LogAtlantisException(exception);
      }

      return result;
    }

    public IGeoLocation RequestGeoLocation
    {
      get 
      {
        if (_requestLocation == null)
        {
          _requestLocation = LoadGeoLocationFromIP(_ipAddress.Value);
        }

        return _requestLocation;
      }
    }

    private IGeoLocation LoadGeoLocationFromIP(string ipAddress)
    {
      IGeoLocation result = null;

      if (ipAddress != null)
      {
        try
        {
          var locationRequest = new IPLocationLookupRequestData(_ipAddress.Value);
          var locationResponse = (IPLocationLookupResponseData)Engine.Engine.ProcessRequest(locationRequest, GeoProviderEngineRequests.IPLocationLookup);
          result = GeoLocation.FromIPLocation(locationResponse.Location);
        }
        catch (Exception ex)
        {
          AtlantisException exception = new AtlantisException("GeoProvider.LoadGeoLocationFromIP", 0, ex.Message, ex.StackTrace);
          Engine.Engine.LogAtlantisException(exception);
        }
      }

      if (result == null)
      {
        result = GeoLocation.FromNotFound();
      }

      return result;
    }

    public IEnumerable<IGeoCountry> Countries
    {
      get { return _geoCountryData.Value.Countries; }
    }

    public bool TryGetCountryByCode(string countryCode, out IGeoCountry country)
    {
      return _geoCountryData.Value.TryGetGeoCountry(countryCode, out country);
    }
  }
}
