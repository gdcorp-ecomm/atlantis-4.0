using System;
using Atlantis.Framework.DBSSellerInterested.Impl.DbsWebService;
using Atlantis.Framework.DBSSellerInterested.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSSellerInterested.Impl
{
  public class DBSSellerInterestedRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DBSSellerInterestedResponseData responseData = null;
      string resultXML = string.Empty;

      DbsWebService.DomainServices DbsWS = null;
      try
      {
        DBSSellerInterestedRequestData requestData = (DBSSellerInterestedRequestData)oRequestData;
        DbsWS = new DomainServices();
        DbsWS.Url = ((WsConfigElement)oConfig).WSURL;
        DbsWS.Timeout = (int)Math.Truncate(requestData.RequestTimeout.TotalMilliseconds);

        resultXML = DbsWS.Status_UpdateSiteSellerInterested(requestData.RequestXml);
        responseData = new DBSSellerInterestedResponseData(resultXML);

      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DBSSellerInterestedResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DBSSellerInterestedResponseData(responseData.ToString(), oRequestData, ex);
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
