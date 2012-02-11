using System;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDGetContextHelp.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetContextHelp.Impl
{
  public class HDVDGetContextHelpRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AriesContextHelpResponse response = null;
      HDVDGetContextHelpResponseData responseData = null;
      HDVDGetContextHelpRequestData request = requestData as HDVDGetContextHelpRequestData;

      HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.GetContextHelp(request.AccountGuid, request.ContextHelpPage, request.ContextHelpField);

            if (response != null)
              responseData = new HDVDGetContextHelpResponseData((response));
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new HDVDGetContextHelpResponseData(request, ex);

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
