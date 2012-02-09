using System;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDSubmitRebootRequest.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDSubmitRebootRequest.Impl
{
  public class HDVDSubmitRebootRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      HDVDSubmitRebootResponseData responseData = null;
     var _requestData = requestData as HDVDSubmitRebootRequestData;

      try
      {
        if (_requestData == null)
        {
          throw new ArgumentNullException("requestData", "requestData cannot be null.");
        }

        HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

        var response = service.SubmitRebootRequest(_requestData.AccountUid.ToString());

        responseData = new HDVDSubmitRebootResponseData(response);

      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new HDVDSubmitRebootResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new HDVDSubmitRebootResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
