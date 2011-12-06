using System;
using Atlantis.Framework.DBSSellerNotInterested.Impl.DbsWebService;
using Atlantis.Framework.DBSSellerNotInterested.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSSellerNotInterested.Impl
{
  public class DBSSellerNotInterestedRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DBSSellerNotInterestedResponseData responseData = null;
      string resultXML = string.Empty;

      DbsWebService.DomainServices DbsWS = null;
      try
      {
        DBSSellerNotInterestedRequestData requestData = (DBSSellerNotInterestedRequestData)oRequestData;
        DbsWS = new DomainServices();
        DbsWS.Url = ((WsConfigElement)oConfig).WSURL;
        DbsWS.Timeout = (int)Math.Truncate(requestData.RequestTimeout.TotalMilliseconds);

        resultXML = DbsWS.DomainBuy_SellerNotInterestedByResourceId(requestData.ResourceId);
        responseData = new DBSSellerNotInterestedResponseData(resultXML);

      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DBSSellerNotInterestedResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DBSSellerNotInterestedResponseData(responseData.ToString(), oRequestData, ex);
      }
      finally
      {
        if (DbsWS != null)
        {
          DbsWS.Dispose();
        }
      }
      return responseData;
    }

  }
}
