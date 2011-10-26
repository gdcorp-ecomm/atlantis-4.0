using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PremiumDNS.Impl.DnsWsApi;
using Atlantis.Framework.PremiumDNS.Interface;

namespace Atlantis.Framework.PremiumDNS.Impl
{
  public class GetPremiumDNSStatusRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      GetPremiumDNSStatusResponseData responseData;
      dnssoapapi oSvc = null;

      try
      {
        bool bResult = false;

        WsConfigElement wsConfig = ((WsConfigElement)config);
        oSvc = NewService(wsConfig.WSURL, ((GetPremiumDNSStatusRequestData)requestData).RequestTimeout);
        
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
        oSvc.custInfo.resellerid = ((GetPremiumDNSStatusRequestData)requestData).PrivateLabelId;
        oSvc.custInfo.execreselleridSpecified = true;

        getPremiumStatusResponseType oResponse = oSvc.getPremiumStatus();
        if (oResponse != null)
        {
          bResult = (oResponse.enabled == 1);
        }

        responseData = new GetPremiumDNSStatusResponseData(bResult);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new GetPremiumDNSStatusResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new GetPremiumDNSStatusResponseData(requestData, ex);
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
      return new dnssoapapi { Url = wsUrl, Timeout = (int)requestTimeout.TotalMilliseconds};
    }
  }
}
