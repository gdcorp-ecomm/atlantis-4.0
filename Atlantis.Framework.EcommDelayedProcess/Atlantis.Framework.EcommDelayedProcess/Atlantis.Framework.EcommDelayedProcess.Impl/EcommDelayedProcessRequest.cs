using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.EcommDelayedProcess.Interface;

namespace Atlantis.Framework.EcommDelayedProcess.Impl
{
  public class EcommDelayedProcessRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommDelayedProcessResponseData responseData = null;

      try
      {
        EcommDelayedProcessRequestData ecomRequest = (EcommDelayedProcessRequestData)requestData;
        using (BasketService.WscgdBasketService oSvc = new BasketService.WscgdBasketService())
        {
          oSvc.Url = ((WsConfigElement)config).WSURL;
          oSvc.Timeout = (int)ecomRequest.RequestTimeout.TotalMilliseconds;
          short result = 0;
          int callResult = oSvc.ProcessDelayedPurchaseResponse(ecomRequest.InvoiceID, ecomRequest.EncryptedResult, out result);
          if (result != 1)
          {
            throw new AtlantisException(requestData, "Delayed Payment Processing", "Could not process Order", ecomRequest.InvoiceID);
          }
          else
          {
            responseData = new EcommDelayedProcessResponseData(result, callResult);
          }
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new EcommDelayedProcessResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EcommDelayedProcessResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
