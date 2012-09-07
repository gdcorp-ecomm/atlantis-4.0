﻿using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCEditOrderEmail.Interface;

namespace Atlantis.Framework.QSCEditOrderEmail.Impl
{
  public class QSCEditOrderEmailRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      responseDetail response = null;
      QSCEditOrderEmailResponseData responseData = null;
      QSCEditOrderEmailRequestData request = requestData as QSCEditOrderEmailRequestData;

      Mobilev10 service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.editOrderEmail(request.AccountUid, request.ShopperID, request.InvoiceId, request.EmailAddress);

            if (response != null)
              responseData = new QSCEditOrderEmailResponseData((response as responseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCEditOrderEmailResponseData(request, ex);
      }
      return responseData;
    }

    #endregion
  }
}
