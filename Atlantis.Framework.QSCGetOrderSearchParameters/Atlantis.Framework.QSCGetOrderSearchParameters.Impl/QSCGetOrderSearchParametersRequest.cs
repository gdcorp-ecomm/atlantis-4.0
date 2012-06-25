using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCGetOrderSearchParameters.Interface;

namespace Atlantis.Framework.QSCGetOrderSearchParameters.Impl
{
  public class QSCGetOrderSearchParametersRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      getOrderSearchParametersResponseDetail response = null;
      QSCGetOrderSearchParametersResponseData responseData = null;
      QSCGetOrderSearchParametersRequestData request = requestData as QSCGetOrderSearchParametersRequestData;

      Mobile service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.getOrderSearchParameters();

            if (response != null)
              responseData = new QSCGetOrderSearchParametersResponseData((response as getOrderSearchParametersResponseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCGetOrderSearchParametersResponseData(request, ex);
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
