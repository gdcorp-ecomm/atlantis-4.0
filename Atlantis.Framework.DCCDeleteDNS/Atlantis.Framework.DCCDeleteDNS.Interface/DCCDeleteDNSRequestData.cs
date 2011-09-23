using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCDeleteDNS.Interface
{
  public class DCCDeleteDNSRequestData : RequestData
  {
    public String DomainName { get; private set; }

    public int PrivateLabelID { get; private set; }

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

    public List<DnsRecordType> Records { get; private set; }

    public DCCDeleteDNSRequestData(string shopperId,
                                    string sourceUrl,
                                    string orderId,
                                    string pathway,
                                    int pageCount,
                                    int privateLabelID,
                                    bool isManagerOrigin,
                                    string domainName) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Records = new List<DnsRecordType>();
      DomainName = domainName;
      PrivateLabelID = privateLabelID;
      _isManagerOrigin = isManagerOrigin;
    }

    public void addRecord(DnsRecordType record)
    {
      Records.Add(record);
    }

    public override string GetCacheMD5()
    {
      throw new Exception("DCCCreateDNS is not a cacheable request.");
    }
  }
}
