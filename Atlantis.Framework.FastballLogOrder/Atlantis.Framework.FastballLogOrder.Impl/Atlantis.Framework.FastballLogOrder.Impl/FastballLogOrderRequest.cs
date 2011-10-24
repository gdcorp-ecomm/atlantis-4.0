using System;
using Atlantis.Framework.FastballLogOrder.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballLogOrder.Impl
{
  public class FastballLogOrderRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      FastballLogOrderResponseData oResponseData;
      try
      {
        FastballLogOrderRequestData oLogRequest = (FastballLogOrderRequestData)oRequestData;
        FastballOrderTracking.Order oSvc = new FastballOrderTracking.Order();
        oSvc.Url = ((WsConfigElement)oConfig).WSURL;
        Guid newGuid;
        try
        {
          if (string.IsNullOrEmpty(oLogRequest.Pathway))
            newGuid = new Guid();
          else
            newGuid = new Guid(oLogRequest.Pathway);
        }
        catch
        {
          newGuid = new Guid();
        }

        if(string.IsNullOrEmpty((oLogRequest.BasketType)))
        {
          oSvc.LogOrder(newGuid, oLogRequest.OrderID, oLogRequest.PageCount);
        }
        else
        {
          oSvc.LogOrderWithType(newGuid, oLogRequest.OrderID, oLogRequest.PageCount, oLogRequest.BasketType);
        }
        
        oResponseData = new FastballLogOrderResponseData(true);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new FastballLogOrderResponseData(false, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new FastballLogOrderResponseData(false, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
