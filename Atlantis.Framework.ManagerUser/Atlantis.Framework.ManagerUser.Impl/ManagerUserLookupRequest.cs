using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ManagerUser.Impl.ManagerLookupWS;
using Atlantis.Framework.ManagerUser.Interface;

namespace Atlantis.Framework.ManagerUser.Impl
{
  public class ManagerUserLookupRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      ManagerUserLookupResponseData oResponseData = null;
      string responseXml = string.Empty;

      try
      {
        using (LookupService lookupService = new LookupService())
        {
          ManagerUserLookupRequestData request = (ManagerUserLookupRequestData)oRequestData;
          lookupService.Url = ((WsConfigElement)oConfig).WSURL;
          lookupService.Timeout = (int)oRequestData.RequestTimeout.TotalMilliseconds;
          responseXml = lookupService.GetUserMappingXml(request.Domain, request.UserId);
          oResponseData = new ManagerUserLookupResponseData(responseXml);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new ManagerUserLookupResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new ManagerUserLookupResponseData(responseXml, (ManagerUserLookupRequestData)oRequestData, ex);
     } 

      return oResponseData;
    }

    #endregion
  }
}
