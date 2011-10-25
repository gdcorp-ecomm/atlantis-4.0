using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.LogOfferImpression.Interface;

namespace Atlantis.Framework.LogOfferImpression.Impl
{
  public class LogOfferImpressionRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      LogOfferImpressionResponseData responseData = null;

      try
      {
        LogOfferImpressionRequestData logOfferImpressionData = (LogOfferImpressionRequestData)oRequestData;
        SmartOfferService.SmartOffers logOfferImpressionWS = new SmartOfferService.SmartOffers();
        logOfferImpressionWS.Url = ((WsConfigElement)oConfig).WSURL;
        logOfferImpressionWS.Timeout = (int)logOfferImpressionData.RequestTimeout.TotalMilliseconds;
        logOfferImpressionWS.LogOfferImpression(
          logOfferImpressionData.ShopperID,
          logOfferImpressionData.FbiOfferIdList,
          logOfferImpressionData.ApplicationID,
          logOfferImpressionData.ImpressionDate,
          logOfferImpressionData.VisitGuid,
          logOfferImpressionData.PageCount);

        responseData = new LogOfferImpressionResponseData(string.Empty);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new LogOfferImpressionResponseData(string.Empty, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new LogOfferImpressionResponseData(string.Empty, oRequestData, ex);
      }

      return responseData;
    }
    #endregion
  }
}
