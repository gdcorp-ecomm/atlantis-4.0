using System;
using Atlantis.Framework.HDVD.Interface;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDGetAccountSummary.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.HDVDGetAccountSummary.Impl.Aries;

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


 HCCAPIServiceAries service = new HCCAPIServiceAries();
       
      try
      {
        using (service)
        {

          service.Url = ((WsConfigElement)config).WSURL;
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
            if (request.AccountGuid != Guid.Empty)
            {
              response = service.GetAccountSummary(request.AccountGuid.ToString());

              if (response != null)
              {
                AriesAccountSummaryInfo asi = response.AccountSummary;
                var convertedAsi =
                  (HDVDAccountSummaryInfo)
                  HDVDObjectConverter<HDVD.Interface.HDVDAccountSummaryInfo>.Convert(asi, typeof(HDVDAccountSummaryInfo));
                responseData = new HDVDGetAccountSummaryResponseData(response.Status,
                                                                     response.StatusCode,
                                                                     response.Message,
                                                                     convertedAsi,
                                                                     response.ResellerID);

              }
              else
              {
                responseData = new HDVDGetAccountSummaryResponseData("error",
                                                                     -1,
                                                                     "Invalid response object recieved.",
                                                                     null,
                                                                     -1);
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
