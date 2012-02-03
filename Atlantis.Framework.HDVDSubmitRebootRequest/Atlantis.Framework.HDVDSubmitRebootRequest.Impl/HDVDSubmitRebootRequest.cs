using System;
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

        var service = new Aries.HCCAPIServiceAries();
        var response = service.SubmitRebootRequest(_requestData.AccountUid.ToString());

        responseData = new HDVDSubmitRebootResponseData(response.Status, response.Message, response.StatusCode);

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
