using System;
using System.Collections.Generic;
using Atlantis.Framework.DCCDeleteDNS.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCDeleteDNS.Impl
{
  public class DCCDeleteDNSRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCDeleteDNSResponseData responseData;
      string responseXml = string.Empty;
      DnsApi.dnssoapapi dnsApi = null;

      try
      {
        DCCDeleteDNSRequestData oRequest = (DCCDeleteDNSRequestData)oRequestData;

        dnsApi = new DnsApi.dnssoapapi();
        dnsApi.Url = ((WsConfigElement)oConfig).WSURL;
        dnsApi.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;

        DnsApi.authDataType oAuth = new DnsApi.authDataType();
        oAuth.clientid = "gdmobile";
        dnsApi.clientAuth = oAuth;

        DnsApi.custDataType oCust = new DnsApi.custDataType();
        oCust.shopperid = oRequest.ShopperID;
        oCust.resellerid = oRequest.PrivateLabelID;
        oCust.origin = oRequest.Origin;
        dnsApi.custInfo = oCust;

        DnsApi.booleanResponseType oResult = dnsApi.deleteRecords(oRequest.DomainName, CreateDnsRecordArray(oRequest));

        if (oResult.result)
        {
          responseData = new DCCDeleteDNSResponseData(true);
        }
        else
        {
          List<string> errorList = new List<string>();
          foreach (DnsApi.responseinfoType responseInfo in oResult.responseinfo)
          {
            string sError = "";

            foreach (string error in responseInfo.info)
            {
              sError += error;
            }
            errorList.Add(sError);
          }

          responseData = new DCCDeleteDNSResponseData(errorList);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCDeleteDNSResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCDeleteDNSResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        if(dnsApi != null)
        {
          dnsApi.Dispose();
        }
      }

      return responseData;
    }

    private static DnsApi.rrecordType[] CreateDnsRecordArray(DCCDeleteDNSRequestData request)
    {
      List<DnsApi.rrecordType> oRecords = new List<DnsApi.rrecordType>();

      foreach (DnsRecordType record in request.Records)
      {
        DnsApi.rrecordType dnsRecord = new DnsApi.rrecordType();
        dnsRecord.attributeUid = record.AttributeUid;
        dnsRecord.data = record.Data;
        dnsRecord.name = record.Name;
        dnsRecord.port = record.Port;
        dnsRecord.priority = record.Priority;
        dnsRecord.protocol = record.Protocol;
        dnsRecord.service = record.Service;
        dnsRecord.status = record.Status;
        dnsRecord.ttl = record.TTL;
        dnsRecord.type = record.Type;
        dnsRecord.weight = record.Weight;
        oRecords.Add(dnsRecord);
      }

      return oRecords.ToArray();
    }
  }
}
