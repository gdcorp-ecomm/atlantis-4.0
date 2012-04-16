using System;
using Atlantis.Framework.DNSSECGetStatus.Impl.DnsSecWS;
using Atlantis.Framework.DNSSECGetStatus.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DNSSECGetStatus.Impl
{
  public class DNSSECGetStatusRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DNSSECGetStatusResponseData responseData = null;

      try
      {
        DNSSECGetStatusRequestData request = (DNSSECGetStatusRequestData)requestData;

        dnssoapapi dnsSecStatusWS = new dnssoapapi();
        dnsSecStatusWS.Url = ((WsConfigElement)config).WSURL;
        dnsSecStatusWS.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        dnsSecStatusWS.custInfo = new custDataType();
        dnsSecStatusWS.custInfo.shopperid = request.ShopperID;
        dnsSecStatusWS.custInfo.resellerid = request.PrivateLabelId;
        dnsSecStatusWS.clientAuth = new authDataType();
        string clientId = config.GetConfigValue("ApplicationName");
        if (string.IsNullOrEmpty(clientId))
        {
          throw new Exception("Atlantis Config file must specify an ApplicationName ConfigValue");
        }
        dnsSecStatusWS.clientAuth.clientid = clientId;

        dnssecStatusType wsResponse = dnsSecStatusWS.getDNSSECStatusByShopper();

        if (wsResponse != null)
        {
          responseData = new DNSSECGetStatusResponseData(wsResponse.usedzones, wsResponse.totalzones);
        }
        else
        {
          responseData = new DNSSECGetStatusResponseData(requestData, new Exception("Error invoking dnssoapapi web service"));
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new DNSSECGetStatusResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new DNSSECGetStatusResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
