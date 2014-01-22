using Atlantis.Framework.Geo.Impl.IPLookup;
using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Geo.Impl
{
  // Possible atlantis.config entry - remove this before peer review
  // <ConfigElement progid="Atlantis.Framework.Geo.Impl.IPLocationLookupRequest" assembly="Atlantis.Framework.Geo.Impl.dll" request_type="###" />

  public class IPLocationLookupRequest : IRequest
  {
    static LocationDataIPv4 _locationData = null;
    static string _locationErrorData;

    static IPLocationLookupRequest()
    {
      LoadLocationData();
    }

    static void LoadLocationData()
    {
      _locationData = null;
      _locationErrorData = IPLookupDataFiles.LocationFile + ":" + IPLookupDataFiles.PathType.ToString();

      try
      {
        string filePath = GeoFilePath.GetFilePath(IPLookupDataFiles.LocationFile);
        _locationData = new LocationDataIPv4(filePath);
      }
      catch (Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        AtlantisException aex = new AtlantisException("Geo.Impl.LoadLocationData", 0, message, _locationErrorData);
        Engine.Engine.LogAtlantisException(aex);
      }
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = IPLocationLookupResponseData.NotFound;
      IPLocationLookupRequestData request = (IPLocationLookupRequestData)requestData;

      try
      {
        if (_locationData != null)
        {
          IPLocation location = _locationData.GetLocation(request.IpAddress);
          result = IPLocationLookupResponseData.FromIPLocation(location);
        }
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException("IPLocationLookupRequest.RequestHandler", 0, ex.Message + ex.StackTrace, requestData.ToXML());
        Engine.Engine.LogAtlantisException(exception);
      }

      return result;
    }
  }
}
