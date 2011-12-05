using System;
using Atlantis.Framework.DBSGetDomainShopper.Impl.DbsWebService;
using Atlantis.Framework.DBSGetDomainShopper.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSGetDomainShopper.Impl
{
  public class DBSGetDomainShopperRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DBSGetDomainShopperResponseData responseData = null;
      string resultXML = string.Empty;

      DbsWebService.DomainServices DbsWS = null;
      try
      {
        DBSGetDomainShopperRequestData requestData = (DBSGetDomainShopperRequestData)oRequestData;
        DbsWS = new DomainServices();
        DbsWS.Url = ((WsConfigElement)oConfig).WSURL;
        DbsWS.Timeout = (int)Math.Truncate(requestData.RequestTimeout.TotalMilliseconds);

        resultXML = DbsWS.GetDomainShopperByLegalResourceId(requestData.ResourceId);
        responseData = new DBSGetDomainShopperResponseData(resultXML);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DBSGetDomainShopperResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DBSGetDomainShopperResponseData(responseData.ToString(), oRequestData, ex);
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
