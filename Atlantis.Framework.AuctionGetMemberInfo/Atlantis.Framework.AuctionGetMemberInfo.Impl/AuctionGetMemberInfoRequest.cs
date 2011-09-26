using System;
using Atlantis.Framework.AuctionGetMemberInfo.Impl.AuctionMemberInfoWS;
using Atlantis.Framework.AuctionGetMemberInfo.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionGetMemberInfo.Impl
{
  public class AuctionGetMemberInfoRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {

      IResponseData oReponseData = null;

      try
      {
        AuctionGetMemberInfoRequestData request = (AuctionGetMemberInfoRequestData)requestData;
        using (trpAdminUtilsService auctionsMemberInfo = new trpAdminUtilsService())
        {
          auctionsMemberInfo.Url = ((WsConfigElement)config).WSURL;
          auctionsMemberInfo.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          string responseXml = auctionsMemberInfo.GetMemberInfoByShopper(requestData.ShopperID);
          oReponseData = new AuctionGetMemberInfoResponseData(responseXml);
        }
      }
      catch (Exception ex)
      {
        oReponseData = new AuctionGetMemberInfoResponseData(requestData, ex);
      }

      return oReponseData;

    }

    #endregion
  }
}
