using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PremiumDNS.Impl.DnsWsApi;
using Atlantis.Framework.PremiumDNS.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.PremiumDNS.Impl
{
  public class GetPremiumDNSDefaultNameserversRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      string[] nameserversDefault;
      Dictionary<string, string> nameserversByTld;
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
        nameserverArrayType results = oSvc.getDefaultNameServers();
        nameserversDefault = results.nameservers;
        responseData = new GetPremiumDNSDefaultNameServersResponseData(nameserversDefault, 
          GetNameserversByTld(results.tldnameservers));       
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

    private Dictionary<string, string[]> GetNameserversByTld(nameserversByTldType[] array)
    {
      Dictionary<string, string[]> dict = new Dictionary<string, string[]>();

      foreach (nameserversByTldType element in array)
      {
        if (element.nameservers.Length > 0)
        {
          dict[element.tld] = element.nameservers;
        }
      }

      return dict;
    }

    #endregion
  }
}
