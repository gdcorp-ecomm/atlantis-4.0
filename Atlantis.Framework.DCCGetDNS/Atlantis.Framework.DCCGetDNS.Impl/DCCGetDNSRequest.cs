using System;
using System.Collections.Generic;
using Atlantis.Framework.DCCGetDNS.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetDNS.Impl
{
  public class DCCGetDNSRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCGetDNSResponseData responseData = null;
      string responseXml = string.Empty;

      DnsApi.dnssoapapi oDnsApi = new DnsApi.dnssoapapi();

      try
      {

        DCCGetDNSRequestData oRequest = (DCCGetDNSRequestData)oRequestData;
        oDnsApi.Url = ((WsConfigElement)oConfig).WSURL;
        oDnsApi.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;


        DnsApi.authDataType oAuth = new DnsApi.authDataType();
        oAuth.clientid = "gdmobile";
        oDnsApi.clientAuth = oAuth;

        DnsApi.custDataType oCust = new DnsApi.custDataType();
        oCust.shopperid = oRequest.ShopperID;
        oCust.resellerid = oRequest.PrivateLabelID;
        oCust.origin = oRequest.Origin;
        oDnsApi.custInfo = oCust;

        DnsApi.rrecordType[] oRecords = oDnsApi.getRRecords(oRequest.DomainName, oRequest.Type);

        List<DnsRecordType> listRecords = new List<DnsRecordType>();
        foreach (DnsApi.rrecordType oRecord in oRecords)
        {
          DnsRecordType localRecord = new DnsRecordType();
          localRecord.Type = oRecord.type;
          localRecord.Status = oRecord.status;
          localRecord.Name = oRecord.name;
          localRecord.AttributeUid = oRecord.attributeUid;
          localRecord.Data = oRecord.data;
          localRecord.Service = oRecord.service;
          localRecord.Protocol = oRecord.protocol;
          localRecord.Port = oRecord.port;
          localRecord.Weight = oRecord.weight;
          localRecord.Priority = oRecord.priority;
          localRecord.TTL = oRecord.ttl;
          listRecords.Add(localRecord);
        }

        responseData = new DCCGetDNSResponseData(oRequest.DomainName, listRecords);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCGetDNSResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCGetDNSResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        oDnsApi.Dispose();
      }

      return responseData;
    }

    #endregion

  }
}
