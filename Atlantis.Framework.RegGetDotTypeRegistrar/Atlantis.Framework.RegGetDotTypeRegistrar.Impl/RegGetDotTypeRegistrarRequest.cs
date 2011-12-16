using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RegGetDotTypeRegistrar.Interface;

namespace Atlantis.Framework.RegGetDotTypeRegistrar.Impl
{
  public class RegGetDotTypeRegistrarRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement configElement)
    {
      IResponseData responseData = null;
      string responseXML = String.Empty;

 
      try
      {
        using (RegistrationApiWebSvc.RegistrationApiWebSvc ws = new RegistrationApiWebSvc.RegistrationApiWebSvc())
        {

          RegGetDotTypeRegistrarRequestData request = (RegGetDotTypeRegistrarRequestData)requestData;

          if (request.IsValid)
          {
            WsConfigElement ce = (WsConfigElement)configElement;
            string requestXml = request.ToXML();
            ws.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
            ws.Url = ((WsConfigElement)ce).WSURL;
            responseXML = ws.GetTLDAPI(requestXml);

            if (!string.IsNullOrEmpty(responseXML))
            {
              responseData = new RegGetDotTypeRegistrarResponseData(responseXML);
            }
            else
            {
              throw new AtlantisException(requestData,
                                          "Framework: RegGetDotTypeRegistrarRequest.RequestHandler",
                                          "Invalid request, null or empty string returned.",
                                          string.Empty);
            }
          }
          else
          {
            throw new AtlantisException(requestData,
                                        "Framework: RegGetDotTypeRegistrarRequest.RequestHandler",
                                        "Invalid request. DotTypes list cannot be empty.",
                                        string.Empty);
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new RegGetDotTypeRegistrarResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new RegGetDotTypeRegistrarResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
