﻿using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Helpers;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCUpdateContact.Interface;

namespace Atlantis.Framework.QSCUpdateContact.Impl
{
  public class QSCUpdateContactRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      responseDetail response = null;
      QSCUpdateContactResponseData responseData = null;
      QSCUpdateContactRequestData request = requestData as QSCUpdateContactRequestData;

      Mobilev10 service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.updateContact(request.AccountUid, request.ShopperID, request.InvoiceId, request.Contact);

            if (response != null)
              responseData = new QSCUpdateContactResponseData((response as responseDetail));

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new QSCUpdateContactResponseData(request, ex);
      }
      return responseData;
    }

    #endregion
  }
}
