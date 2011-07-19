using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.LogOfferClick.Interface;

namespace Atlantis.Framework.LogOfferClick.Impl
{
  public class LogOfferClickRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      string response = string.Empty;
      LogOfferClickResponseData responseData = null;
      int offerID = 0;

      try
      {
        LogOfferClickRequestData logOfferClickData = (LogOfferClickRequestData)oRequestData;

        using (SmartOffersService.SmartOffers logOfferClickWS = new SmartOffersService.SmartOffers())
        {
          logOfferClickWS.Url = ((WsConfigElement)oConfig).WSURL;
          logOfferClickWS.Timeout = (int)logOfferClickData.RequestTimeout.TotalMilliseconds;

          int.TryParse(logOfferClickData.FbiOfferID, out offerID);
          if (offerID > 0)
          {
            logOfferClickWS.LogOfferClick(
            offerID,
            logOfferClickData.ShopperID,
            logOfferClickData.ClickDate,
            logOfferClickData.ApplicationID,
            logOfferClickData.VisitGuid,
            logOfferClickData.PageCount);
          }
          else
          {
            throw new AtlantisException(oRequestData, "LogOfferClickRequest::RequestHandler", "Invalid offerID Provided", logOfferClickData.FbiOfferID);
          }
          responseData = new LogOfferClickResponseData(response);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new LogOfferClickResponseData(response, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new LogOfferClickResponseData(response, oRequestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}
