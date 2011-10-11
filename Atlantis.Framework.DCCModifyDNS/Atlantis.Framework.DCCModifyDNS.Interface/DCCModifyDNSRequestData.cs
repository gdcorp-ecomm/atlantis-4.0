using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCModifyDNS.Interface
{
  public class DCCModifyDNSRequestData: RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(12);

    

    

    public DCCModifyDNSRequestData(string shopperId,
                                    string sourceUrl,
                                    string orderId,
                                    string pathway,
                                    int pageCount,
                                    int privateLabelID,
                                    bool isManagerOrigin,
                                    string domainName)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _records = new List<DnsRecordType>();
      _domainName = domainName;
      _privateLabelID = privateLabelID;
      _isManagerOrigin = isManagerOrigin;
      RequestTimeout = _requestTimeout;
    }

    public void addRecord( DnsRecordType record )
    {
      _records.Add(record);
    }

    private string _domainName;
    public String DomainName
    {
      get { return _domainName; }
    }

    private int _privateLabelID;
    public int PrivateLabelID
    {
      get { return _privateLabelID; }
    }

    private bool _isManagerOrigin;
    public string Origin
    {
      get 
      {
        string sRetVal = "Customer";
        if (_isManagerOrigin)
          sRetVal = "Manager";
        return sRetVal; 
      }
    }

    private List<DnsRecordType> _records;
    public List<DnsRecordType> Records
    {
      get { return _records; }
    }

    public override string GetCacheMD5()
    {
      throw new Exception("DCCModifyDNS is not a cacheable request.");
    }
  }
}
