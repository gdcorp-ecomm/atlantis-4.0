using System;
using Atlantis.Framework.DCCGetDomainByShopper.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetDomainByShopper.Impl
{
  public class DCCGetDomainByShopperRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCGetDomainByShopperResponseData responseData;
      string responseXml = string.Empty;

      try
      {
        using (DsWeb.RegCheckDomainStatusWebSvcService oDsWeb = new DsWeb.RegCheckDomainStatusWebSvcService())
        {
          DCCGetDomainByShopperRequestData oRequest = (DCCGetDomainByShopperRequestData)oRequestData;
          oDsWeb.Url = ((WsConfigElement)oConfig).WSURL;
          oDsWeb.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
          responseXml = oDsWeb.GetDCCDomainList(oRequest.ToXML());
          responseData = new DCCGetDomainByShopperResponseData(responseXml, oRequest.UseMaxdateAsDefaultForExpirationDate);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCGetDomainByShopperResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCGetDomainByShopperResponseData(responseXml, oRequestData, ex);
      }

      return responseData;
    }

    #endregion

  }
}
