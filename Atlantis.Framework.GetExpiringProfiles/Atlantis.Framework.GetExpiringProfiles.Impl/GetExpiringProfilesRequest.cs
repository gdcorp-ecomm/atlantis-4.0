using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.GetExpiringProfiles.Interface;

namespace Atlantis.Framework.GetExpiringProfiles.Impl
{
  public class GetExpiringProfilesRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      GetExpiringProfilesResponseData responseData = null;

      try
      {
        GetExpiringProfilesRequestData expiringProfilesRequestData = (GetExpiringProfilesRequestData)oRequestData;

        using (PPWebSvcService.PPWebSvcService expiringProfilesWS = new PPWebSvcService.PPWebSvcService())
        {
          expiringProfilesWS.Url = ((WsConfigElement)oConfig).WSURL;
          expiringProfilesWS.Timeout = (int)expiringProfilesRequestData.RequestTimeout.TotalMilliseconds;

          string errorStr;
          string response = expiringProfilesWS.GetExpiringProfiles(
            string.Empty,
            expiringProfilesRequestData.ShopperID,
            expiringProfilesRequestData.DaysBefore,
            expiringProfilesRequestData.DaysAfter,
            out errorStr);

          if (!string.IsNullOrEmpty(errorStr))
          {
            throw new AtlantisException(oRequestData, "GetExpiringProfiles::RequestHandler", "Web Service Error:", errorStr);
          }
          if (string.IsNullOrEmpty(response))
          {
            throw new AtlantisException(oRequestData, "GetExpiringProfiles::RequestHandler", "GetExpiringProfiles returns empty or null string.", String.Empty);
          }

          responseData = new GetExpiringProfilesResponseData(oRequestData, response);
        }
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new GetExpiringProfilesResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new GetExpiringProfilesResponseData(oRequestData, ex);
      }
       
      return responseData;
    }
  }
}
