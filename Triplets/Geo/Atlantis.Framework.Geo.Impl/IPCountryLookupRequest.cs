using Atlantis.Framework.Geo.Impl.IPLookup;
using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Geo.Impl
{
  public class IPCountryLookupRequest : IRequest
  {
    static CountryDataIPv4 _countryData = null;
    static string _countryErrorData;

    static IPCountryLookupRequest()
    {
      LoadCountryData();
    }

    static void LoadCountryData()
    {
      _countryData = null;
      _countryErrorData = IPLookupDataFiles.CountryFile + ":" + IPLookupDataFiles.PathType.ToString();

      try
      {
        string filePath = GeoFilePath.GetFilePath(IPLookupDataFiles.CountryFile);
        _countryData = new CountryDataIPv4(filePath);
      }
      catch (Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        AtlantisException aex = new AtlantisException("Geo.Impl.LoadCountryData", 0, message, _countryErrorData);
        Engine.Engine.LogAtlantisException(aex);
      }
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = IPCountryLookupResponseData.NotFound;
      IPCountryLookupRequestData request = (IPCountryLookupRequestData)requestData;

      try
      {
        if (_countryData != null)
        {
          string countryCode = _countryData.GetCountry(request.IpAddress);
          if (!string.IsNullOrEmpty(countryCode))
          {
            result = IPCountryLookupResponseData.FromCountry(countryCode);
          }
        }
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException("IPCountryLookupRequest.RequestHandler", 0, ex.Message + ex.StackTrace, requestData.ToXML());
        Engine.Engine.LogAtlantisException(exception);
      }

      return result;
    }
  }
}
