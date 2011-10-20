using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RegVendorDomainSearch.Interface;
using Atlantis.Framework.RegVendorDomainSearch.Impl.gdDPPSearch;

namespace Atlantis.Framework.RegVendorDomainSearch.Impl
{
  public class RegVendorDomainSearchRequest : IRequest
  {
    private const int _MAX_SERVICETIMEOUT_MILLISECONDS = 300000;// in miliseconds

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      RegVendorDomainSearchResponseData responseData = null;
      string responseXml = string.Empty;

      try
      {
        gdDppSearchWS service = new gdDppSearchWS();
        RegVendorDomainSearchRequestData request = (RegVendorDomainSearchRequestData)oRequestData;
        string url = ((WsConfigElement)oConfig).WSURL;
        string rqXml = oRequestData.ToXML();
        service.Url = url;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        if (request._requestTimeout.Milliseconds > _MAX_SERVICETIMEOUT_MILLISECONDS)
        {
          service.Timeout = _MAX_SERVICETIMEOUT_MILLISECONDS;
        }

        responseXml = service.dppDomainSearch(rqXml);
        responseData = new RegVendorDomainSearchResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new RegVendorDomainSearchResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new RegVendorDomainSearchResponseData(responseXml, oRequestData, ex);
      }

      return responseData;
    }
  }
}
