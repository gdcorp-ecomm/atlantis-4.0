using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RegGetDotTypeProductIdList.Interface;

namespace Atlantis.Framework.RegGetDotTypeProductIdList.Impl
{
  public class RegGetDotTypeProductIdListRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement configElement)
    {
      IResponseData responseData = null;
      string responseXML = String.Empty;

      

      try
      {
        using (RegistrationApiWebSvc.RegistrationApiWebSvc ws = new RegistrationApiWebSvc.RegistrationApiWebSvc())
        {
          RegGetDotTypeProductIdListRequestData request = (RegGetDotTypeProductIdListRequestData)requestData;

          WsConfigElement ce = (WsConfigElement)configElement;
          string requestXml = request.ToXML();
          ws.Url = ((WsConfigElement)ce).WSURL;
          ws.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          responseXML = ws.GetProductIdList(requestXml);

          if (!string.IsNullOrEmpty(responseXML))
          {
            responseData = new RegGetDotTypeProductIdListResponseData(responseXML);
          }
          else
          {
            throw new AtlantisException(requestData,
                                        "Framework: RegGetDotTypeProductIdListRequest.RequestHandler",
                                        "Invalid request, null or empty string returned",
                                        string.Empty);
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new RegGetDotTypeProductIdListResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new RegGetDotTypeProductIdListResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
