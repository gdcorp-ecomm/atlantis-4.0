using System;
using Atlantis.Framework.FastballLogClick.Impl.SmartOffersWS;
using Atlantis.Framework.FastballLogClick.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballLogClick.Impl
{
  public class FastballLogClickRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;

      try
      {
        FastballLogClickRequestData requestData = (FastballLogClickRequestData)oRequestData;        
        
        SmartOffers offersWS = new SmartOffers();
        offersWS.Url = ((WsConfigElement)oConfig).WSURL;
        offersWS.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
        
        offersWS.LogOfferClick(requestData.FBOfferId, requestData.ShopperID, DateTime.Now,
                                                    requestData.ApplicationId, requestData.VisitGuid,
                                                    requestData.PageCount);
        result = new FastballLogClickResponseData
                   {
                     IsSuccess = true
                   };

      }
      catch (Exception ex)
      {
        result = new FastballLogClickResponseData(oRequestData, ex);
      }

      return result;
    }

    #endregion
  }
}
