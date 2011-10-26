using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PremiumDNS.Impl.DnsWsApi;
using Atlantis.Framework.PremiumDNS.Interface;

namespace Atlantis.Framework.PremiumDNS.Impl
{
  public class GetPremiumDNSDefaultNameserversRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string[] results;
      WsConfigElement wsConfig = ((WsConfigElement)config);

      dnssoapapi oSvc = null;
      GetPremiumDNSDefaultNameServersResponseData responseData;

      try
      {
        oSvc = NewService(wsConfig.WSURL, ((GetPremiumDNSDefaultNameServersRequestData)requestData).RequestTimeout);

        if (oSvc.clientAuth == null)
        {
          oSvc.clientAuth = new authDataType();
        }
        oSvc.clientAuth.clientid = config.GetConfigValue("ClientId"); 
        if (oSvc.custInfo == null)
        {
          oSvc.custInfo = new custDataType();
        }
        oSvc.custInfo.shopperid = requestData.ShopperID;
        oSvc.custInfo.resellerid = ((GetPremiumDNSDefaultNameServersRequestData)requestData).PrivateLabelId;
        oSvc.custInfo.execreselleridSpecified = true;

        results = oSvc.getDefaultNameServers();
        responseData = new GetPremiumDNSDefaultNameServersResponseData(results);

       
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new GetPremiumDNSDefaultNameServersResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new GetPremiumDNSDefaultNameServersResponseData(requestData, ex);
      }
      finally
      {
        if(oSvc != null)
        {
          oSvc.Dispose();
        }
      }
      
      return responseData;
    }

    private static dnssoapapi NewService(string wsUrl, TimeSpan requestTimeout)
    {
      return new dnssoapapi { Url = wsUrl, Timeout = (int)requestTimeout.TotalMilliseconds };
    }

    #endregion
  }
}
