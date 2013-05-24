using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using System;
using System.Web;

namespace Atlantis.Framework.Providers.Geo
{
  public class GeoProvider : ProviderBase, IGeoProvider
  {
    Lazy<ISiteContext> _siteContext;
    Lazy<IProxyContext> _proxyContext;
    Lazy<string> _requestCountryCode;
    Lazy<CountryResponseData> _countries;

    public GeoProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => { return Container.Resolve<ISiteContext>(); });
      _proxyContext = new Lazy<IProxyContext>(() => { return LoadProxyContext(); });
      _requestCountryCode = new Lazy<string>(() => { return DetermineRequestCountryCode(); });
      _countries = new Lazy<CountryResponseData>(() => { return LoadCountries(); });
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
      string ipAddress = GetRequestIP();

      if (!string.IsNullOrEmpty(ipAddress))
      {
        result = LookupCountryByIP(ipAddress);      
      }

      return result;
    }

    private string LookupCountryByIP(string ipAddress)
    {
      string result = "us";

      var request = new IPCountryLookupRequestData(ipAddress);
      var response = (IPCountryLookupResponseData)Engine.Engine.ProcessRequest(request, GeoProviderEngineRequests.IPCountryLookup);

      if (response.CountryFound)
      {
        result = response.CountryCode;
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
      return countryCode.Equals(_requestCountryCode.Value, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsUserInRegion(int regionTypeId, string regionName)
    {
      bool result = false;
      
      Country country = _countries.Value.FindCountryByCode(_requestCountryCode.Value);
      if (country != null)
      {
        var regionRequest = new RegionRequestData(regionTypeId, regionName);
        var regionResponse = (RegionResponseData)DataCache.DataCache.GetProcessRequest(regionRequest, GeoProviderEngineRequests.Regions);
        result = regionResponse.HasCountry(country.Id);
      }

      return result;
    }
  }
}
