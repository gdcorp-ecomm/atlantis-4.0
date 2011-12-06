using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RegDomainsDbsCheck.Interface;

namespace Atlantis.Framework.RegDomainsDbsCheck.Impl
{
  public class RegDomainsDbsCheckRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement configElement)
    {
      IResponseData responseData = null;
      string responseXML = String.Empty;

      DbsWebService.DomainServices ws = null;
      try
      {
        RegDomainsDbsCheckRequestData request = (RegDomainsDbsCheckRequestData)requestData;
        WsConfigElement ce = (WsConfigElement)configElement;
        string requestXml = request.ToXML();
        ws = new DbsWebService.DomainServices();
        ws.Timeout = (int)Math.Truncate(request.RequestTimeout.TotalMilliseconds);
        ws.Url = ((WsConfigElement)ce).WSURL;
        responseXML = ws.IsDomainDbsCapableBulk(requestXml);

        if (!string.IsNullOrEmpty(responseXML))
        {
          responseData = new RegDomainsDbsCheckResponseData(responseXML);
        }
        else
        {
          throw new AtlantisException(requestData,
                                      "Framework: RegDomainsDbsCheckRequest.RequestHandler",
                                      "Invalid request, null or empty string returned",
                                      string.Empty);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new RegDomainsDbsCheckResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new RegDomainsDbsCheckResponseData(requestData, ex);
      }
      finally
      {
        if (ws != null)
        {
          ws.Dispose();
        }
      }

      return responseData;
    }
  }
}
