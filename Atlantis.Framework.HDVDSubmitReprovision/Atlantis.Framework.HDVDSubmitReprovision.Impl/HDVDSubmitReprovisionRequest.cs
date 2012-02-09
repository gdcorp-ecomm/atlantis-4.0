using System;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDSubmitReprovision.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDSubmitReprovision.Impl
{
  public class HDVDSubmitReprovisionRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      HDVDSubmitReprovisionResponseData responseData = null;
     var request = requestData as HDVDSubmitReprovisionRequestData;

      try
      {
        HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

        if (request != null)
        {
          var response = service.SubmitReprovision(request.AccountUid, request.ServerName, request.Username, request.Password, request.OSVersion);
          responseData = new HDVDSubmitReprovisionResponseData(response);
        }
        else
        {
          throw new ArgumentNullException("requestData", "Request data is null or empty, or could not be cast to HDVDSubmitReprovisionRequestData.");
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new HDVDSubmitReprovisionResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new HDVDSubmitReprovisionResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
