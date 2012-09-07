using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCGetAccounts.Interface;

namespace Atlantis.Framework.QSCGetAccounts.Impl
{
  public class QSCGetAccountsRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      getAccountResponseDetail response = null;
      QSCGetAccountsResponseData responseData = null;
      QSCGetAccountsRequestData request = requestData as QSCGetAccountsRequestData;

      Mobilev10 service = ServiceHelper.GetServiceReference(((WsConfigElement) config).WSURL);

      try 
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

						response = service.getAccounts(request.ShopperID);

            if (response != null)
              responseData = new QSCGetAccountsResponseData((response as getAccountResponseDetail));
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCGetAccountsResponseData(request, ex);
      }
      return responseData;
    }

    #endregion
  }
}
