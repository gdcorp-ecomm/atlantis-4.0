using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DCCValidateNS.Interface;
using Atlantis.Framework.DCCValidateNS.Impl.DNSApi;

namespace Atlantis.Framework.DCCValidateNS.Impl 
{
  public class DCCValidateNSRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DCCValidateNSResponseData responseData;
      DNSApi.dnsverifyapi oSvc=null;
      try
      {

        DCCValidateNSRequestData orequest= (DCCValidateNSRequestData)requestData;

        WsConfigElement wsConfig = ((WsConfigElement)config);
        oSvc = new DNSApi.dnsverifyapi();
        oSvc.Timeout=(int)orequest.RequestTimeout.TotalMilliseconds;
        oSvc.Url = wsConfig.WSURL;

        if (oSvc.clientAuth == null)
        {
          oSvc.clientAuth = new DNSApi.authDataType();
        }
        oSvc.clientAuth.clientid = config.GetConfigValue("ClientId");
        if (oSvc.custInfo == null)
        {
          oSvc.custInfo = new DNSApi.custDataType();
        }
        oSvc.custInfo.shopperid = requestData.ShopperID;
        oSvc.custInfo.resellerid = ((DCCValidateNSRequestData)requestData).PrivateLabelId;
        oSvc.custInfo.execreselleridSpecified = true;

        validateNSResponseType oResult2 = oSvc.validateNS(orequest.NameServers.ToArray());
        List<DCCValidateNameServerInfo> oResults = new List<DCCValidateNameServerInfo>();
        if (oResult2.nameservers.Length>0)
        {
          foreach (DNSApi.nameServerValType currentType in oResult2.nameservers)
          {
            DCCValidateNameServerInfo oInfo = new DCCValidateNameServerInfo();
            oInfo.NameServer = currentType.nameserver;
            switch (currentType.type)
            {
              case 0:
                oInfo.NameServerType = DCCValidateNameServerInfo.NameServerTypeEnum.THIRDPARTY;
                break;
              case 1:
                oInfo.NameServerType = DCCValidateNameServerInfo.NameServerTypeEnum.GODADDY;
                break;
              case 2:
                oInfo.NameServerType = DCCValidateNameServerInfo.NameServerTypeEnum.PREMIUM;
                break;
              case 3:
                oInfo.NameServerType = DCCValidateNameServerInfo.NameServerTypeEnum.VANITY;
                break;
            }
            if (currentType.status == 0)
            {
              oInfo.Valid = false;
            }
            else
            {
              oInfo.Valid = true;
            }
            oResults.Add(oInfo);
          }
        }
        else
        {
          foreach (string nameServer in orequest.NameServers)
          {
            DCCValidateNameServerInfo oInfo = new DCCValidateNameServerInfo();
            oInfo.NameServer = nameServer;
            oInfo.NameServerType = DCCValidateNameServerInfo.NameServerTypeEnum.THIRDPARTY;
            oInfo.Valid = true;
            oResults.Add(oInfo);
          }
        }
        responseData = new DCCValidateNSResponseData(oResults, oResult2.result, oResult2.@internal, oResult2.errorcode);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCValidateNSResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCValidateNSResponseData(requestData, ex);
      }
      finally
      {
        if (oSvc != null)
        {
          oSvc.Dispose();
        }
      }

      return responseData;
    }
  }
}
