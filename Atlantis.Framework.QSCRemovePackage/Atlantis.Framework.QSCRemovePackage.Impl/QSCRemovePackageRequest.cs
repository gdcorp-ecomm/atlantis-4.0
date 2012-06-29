using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCRemovePackage.Interface;

namespace Atlantis.Framework.QSCRemovePackage.Impl
{
  public class QSCRemovePackageRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      responseDetail response = null;
      QSCRemovePackageResponseData responseData = null;
      QSCRemovePackageRequestData request = requestData as QSCRemovePackageRequestData;

      Mobile service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.removePackage(request.AccountUid, request.InvoiceId, request.PackageId);

            if (response != null)
              responseData = new QSCRemovePackageResponseData((response as responseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCRemovePackageResponseData(request, ex);
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
