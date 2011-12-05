using System;
using Atlantis.Framework.DBSCreateTdnamAuction.Impl.DbsWebService;
using Atlantis.Framework.DBSCreateTdnamAuction.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSCreateTdnamAuction.Impl
{
  public class DBSCreateTdnamAuctionRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DBSCreateTdnamAuctionResponseData responseData = null;
      string resultXML = string.Empty;

      DbsWebService.DomainServices DbsWS = null;
      try
      {
        DBSCreateTdnamAuctionRequestData requestData = (DBSCreateTdnamAuctionRequestData)oRequestData;
        DbsWS = new DomainServices();
        DbsWS.Url = ((WsConfigElement)oConfig).WSURL;
        DbsWS.Timeout = (int)Math.Truncate(requestData.RequestTimeout.TotalMilliseconds);

        resultXML = DbsWS.DomainBuy_CreateTdnamAuction(requestData.RequestXml);
        responseData = new DBSCreateTdnamAuctionResponseData(resultXML);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DBSCreateTdnamAuctionResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DBSCreateTdnamAuctionResponseData(responseData.ToString(), oRequestData, ex);
      }
      finally
      {
        if ( DbsWS != null )
        {
          DbsWS.Dispose();
        }
      }
      return responseData;
    }

  }
}
