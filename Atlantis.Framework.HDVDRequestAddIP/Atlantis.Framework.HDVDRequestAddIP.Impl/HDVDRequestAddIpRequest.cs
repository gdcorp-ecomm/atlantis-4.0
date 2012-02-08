using System;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
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
      HCCAPIServiceAries service = SerivceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

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

        responseData = new HDVDRequestAddIpResponseData(response);
        
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
