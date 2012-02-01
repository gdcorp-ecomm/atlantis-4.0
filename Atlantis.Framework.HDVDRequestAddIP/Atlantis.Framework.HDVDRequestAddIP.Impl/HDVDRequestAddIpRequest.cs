using System;
using Atlantis.Framework.HDVD.Interface;
using Atlantis.Framework.HDVDRequestAddIP.Impl.Aries;
using Atlantis.Framework.HDVDRequestAddIP.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDRequestAddIP.Impl
{
  public class HDVDRequestAddIpRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {

      var request = requestData as HDVDRequestAddIpRequestData;
      HDVDRequestAddIpResponseData responseData;
      var service = new HCCAPIServiceAries();

      try
      {
        if (request == null)
        {
          throw new ArgumentNullException("requestData", "Argument cannot be null.");
        }

        if (request.AccountUid == Guid.Empty)
        {
          throw new ArgumentNullException("requestData.AccountUid", "Argument cannot be null or Guid.Empy");
        }

        AriesHostingResponse response;
        using (service)
        {
          service.Url = ((WsConfigElement)config).WSURL;
          response = service.RequestAdditionalIPAddress(request.AccountUid.ToString());
        }

        if (response != null)
        {
          var convertedResponse = HDVD.Interface.Helpers.HDVDObjectConverter<HDVDHostingResponse>.Convert(response, typeof(HDVDHostingResponse)) as HDVDHostingResponse;
          responseData = new HDVDRequestAddIpResponseData(convertedResponse);
        }
        else
        {
          var errorResponse = new HDVDHostingResponse { Message = "Invalid response object recieved.", Status = "error", StatusCode = -1 };
          responseData = new HDVDRequestAddIpResponseData(errorResponse);
        }
      }
      catch (Exception ex)
      {
        responseData = new HDVDRequestAddIpResponseData(request, ex);
      }
      finally
      {
        service.Dispose();
      }
      return responseData;
    }

    #endregion
  }
}
