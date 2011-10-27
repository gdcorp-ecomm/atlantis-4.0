using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetDNS.Interface
{
  public class DCCGetDNSResponseData : IResponseData
  {
    private string _responseXml;
    private AtlantisException _exception;
    private string _domainName;
    private List<DnsRecordType> _records;

    public DCCGetDNSResponseData(string domainName, List<DnsRecordType> oRecords)
    {
      _domainName = domainName;
      _records = oRecords;
    }

    public DCCGetDNSResponseData(string responseXML, AtlantisException exAtlantis)
    {
      _responseXml = responseXML;
      _exception = exAtlantis;
      _records = new List<DnsRecordType>(1);
    }

    public DCCGetDNSResponseData(string responseXML, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXML;
      _domainName = string.Empty;
      _records = new List<DnsRecordType>(1);

      // If the zonefile was not found, don't throw an exception
      if (!ex.Message.ToLower().Contains("could not find dns zonefile for"))
      {
        _exception = new AtlantisException(oRequestData,
                                           "DCCGetDNSResponseData",
                                           ex.Message,
                                           oRequestData.ToXML());
      }
    }

    public bool DnsZoneFileFound
    {
      get { return _records.Count > 0; }
    }

    public string DomainName
    {
      get { return _domainName; }
    }

    public List<DnsRecordType> Records
    {
      get { return _records; }
    }

    public bool IsSuccess
    {
      get { return (_exception == null && _records.Count > 0); }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseXml;
    }

    #endregion
  }
}
