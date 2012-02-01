using System;
using Atlantis.Framework.HDVDGetSupportedOS.Impl.Aries;
using Atlantis.Framework.HDVDGetSupportedOS.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetSupportedOS.Impl
{
  public class HDVDGetSupportedOSRequest : IRequest 
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = requestData as HDVDGetSupportedOSRequestData;
      HDVDGetSupportedOSResponseData responseData = null; 
      
      HCCAPIServiceAries service = new HCCAPIServiceAries();
      try
      {
        if (request.AccountUid == Guid.Empty)
        {
          throw new ArgumentNullException("request.AccountUid", "Argument cannot be null or Guid.Empy");
        }
        using (service)
        {


          service.Url = ((WsConfigElement)config).WSURL;

          AriesOperatingSystemResponse response = service.GetSupportedOperatingSystems(request.AccountUid.ToString());

          if (response != null)
          {
            responseData = new HDVDGetSupportedOSResponseData(response.Status, response.StatusCode, response.Message,
                                                              response.OperatingSystems.Length > 0
                                                                ? response.OperatingSystems
                                                                : null);
          }
          else
          {
            responseData = new HDVDGetSupportedOSResponseData("error",
                                                              -1,
                                                              "Invalid response object recieved.",
                                                              null);
          }
        }

      }
      catch (Exception ex)
      {
        responseData = new HDVDGetSupportedOSResponseData(requestData, ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }

      return responseData;
    }

    #endregion
  }
}

