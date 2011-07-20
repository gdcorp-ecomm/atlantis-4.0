using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AuctionsAreaBySection.Interface;
using System.Data;

namespace Atlantis.Framework.AuctionsAreaBySection.Impl
{
  public class AuctionsAreaBySectionRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DataSet dataset = null;
      AuctionsAreaBySectionResponseData responseData = null;

      try
      {
        AuctionsAreaBySectionRequestData auctionData = (AuctionsAreaBySectionRequestData)requestData;
        using (TdnamAdminUtilityService.trpAdminUtilsService auctionWS = new TdnamAdminUtilityService.trpAdminUtilsService())
        {
          auctionWS.Url = ((WsConfigElement)config).WSURL;
          auctionWS.Timeout = (int)auctionData.RequestTimeout.TotalMilliseconds;
          dataset = auctionWS.GetMembersAreaBySection(auctionData.ShopperID, auctionData.MembersAreaID, auctionData.ReturnBids);

          if (dataset != null && dataset.Tables.Count > 0)
          {
            responseData = new AuctionsAreaBySectionResponseData(dataset);
          }
          else
          {
            throw new AtlantisException(auctionData, "AuctionsAreaBySectionRequest::RequestHandler", "Null/Empty Dataset returned", "");
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new AuctionsAreaBySectionResponseData(dataset, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new AuctionsAreaBySectionResponseData(dataset, requestData, ex);
      }

      return responseData;

    }
    #endregion
  }
}
