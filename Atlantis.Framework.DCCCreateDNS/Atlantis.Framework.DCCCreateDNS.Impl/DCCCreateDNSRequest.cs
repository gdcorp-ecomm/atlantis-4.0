using System;
using System.Collections.Generic;
using Atlantis.Framework.DCCCreateDNS.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCCreateDNS.Impl
{
  public class DCCCreateDNSRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCCreateDNSResponseData responseData;
      string responseXml = string.Empty;
      DnsApi.dnssoapapi dnsApi = null;

      try
      {
        DCCCreateDNSRequestData oRequest = (DCCCreateDNSRequestData)oRequestData;
        
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

        DnsApi.booleanResponseType oResult = dnsApi.createRecords(oRequest.DomainName, CreateDnsRecordArray( oRequest ) );

        if (oResult.errorcode == 0)
        {
          responseData = new DCCCreateDNSResponseData(true);
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

          responseData = new DCCCreateDNSResponseData(errorList);
        }
      }
      catch (Exception ex)
      {
        responseData = new DCCCreateDNSResponseData(responseXml, oRequestData, ex);
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

    private static DnsApi.rrecordType[] CreateDnsRecordArray(DCCCreateDNSRequestData oRequest)
    {
      List<DnsApi.rrecordType> oRecords = new List<DnsApi.rrecordType>();

      foreach (DnsRecordType record in oRequest.Records)
      {
        DnsApi.rrecordType dnsRecord = new DnsApi.rrecordType();

        dnsRecord.attributeUid = null;
        dnsRecord.type = record.Type;
        dnsRecord.data = record.Data;
        dnsRecord.name = record.Name;
        dnsRecord.protocol = record.Protocol;
        dnsRecord.service = record.Service;
        dnsRecord.status = record.Status;
        
        if(record.Weight > 0)
        {
          dnsRecord.weightSpecified = true;
          dnsRecord.weight = record.Weight;
        }

        if(record.Port > 0)
        {
          dnsRecord.portSpecified = true;
          dnsRecord.port = record.Port;
        }
        
        if(record.Priority > 0)
        {
          dnsRecord.prioritySpecified = true;
          dnsRecord.priority = record.Priority;
        }
        
        if (record.TTL > 0)
        {
          dnsRecord.ttl = record.TTL;
          dnsRecord.ttlSpecified = true;
        }
        
        oRecords.Add(dnsRecord);
      }

      return oRecords.ToArray();
    }
  }
}
