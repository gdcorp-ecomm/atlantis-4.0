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
    readonly Lazy<ISiteContext> _siteContext;
    readonly Lazy<IProxyContext> _proxyContext;
    readonly Lazy<string> _requestCountryCode;
    readonly Lazy<string> _ipAddress;
    readonly Lazy<GeoCountryData> _geoCountryData;

    IGeoLocation _requestLocation;

    public GeoProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _proxyContext = new Lazy<IProxyContext>(LoadProxyContext);
      _requestCountryCode = new Lazy<string>(DetermineRequestCountryCode);
      _ipAddress = new Lazy<string>(GetRequestIP);
      _geoCountryData = new Lazy<GeoCountryData>(() => new GeoCountryData(Container));
    }

    private IProxyContext LoadProxyContext()
    {
      IProxyContext result;
      if (!Container.TryResolve(out result))
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
        result = TryGetCountryCodeFromLocation(out alreadyLoadedCountryCode) ? alreadyLoadedCountryCode : LookupCountryByIP(_ipAddress.Value);
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
        var exception = new AtlantisException("GeoProvider.LookupCountryByIP", 0, ex.Message, ex.StackTrace);
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

      return countryCode.Equals(_requestCountryCode.Value, StringComparison.OrdinalIgnoreCase);
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
        var exception = new AtlantisException("GeoProvider.IsUserInRegion", 0, ex.Message, ex.StackTrace);
        Engine.Engine.LogAtlantisException(exception);
      }

      return result;
    }

    public IGeoLocation RequestGeoLocation
    {
      get { return _requestLocation ?? (_requestLocation = LoadGeoLocationFromIP(_ipAddress.Value)); }
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
          var exception = new AtlantisException("GeoProvider.LoadGeoLocationFromIP", 0, ex.Message, ex.StackTrace);
          Engine.Engine.LogAtlantisException(exception);
        }
      }

      return result ?? GeoLocation.FromNotFound();
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
