using System;
using Atlantis.Framework.HDVD.Interface;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDGetAccountSummary.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetAccountSummary.Impl
{
  public class HDVDGetAccountSummaryRequest : IRequest
  {
    private const string statusSuccess = "success";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AriesAccountSummaryResponse response = null;
      HDVDGetAccountSummaryResponseData responseData = null;
      HDVDGetAccountSummaryRequestData request = requestData as HDVDGetAccountSummaryRequestData;


      HCCAPIServiceAries service = SerivceHelper.GetServiceReference(((WsConfigElement)config).WSURL);
       
      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
            if (request.AccountGuid != Guid.Empty)
            {
              response = service.GetAccountSummary(request.AccountGuid.ToString());

              if (response != null)
              {
                responseData = new HDVDGetAccountSummaryResponseData(response);

              }
            }
            else
            {
              responseData = new HDVDGetAccountSummaryResponseData(request,
                                                                   new ArgumentNullException("request.AccountGuid",
                                                                                             "Account Guid cannot be null or Guid.Empty"));
            }
          }
        }

      }
      catch (Exception ex)
      {
        responseData = new HDVDGetAccountSummaryResponseData(request, ex);
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
