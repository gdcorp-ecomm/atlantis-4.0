using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetDNS.Interface
{
  public class DCCGetDNSRequestData : RequestData
  {
    string _domainName;
    string _type;
    int _privateLabelID;
    bool _isManagerOrigin;

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public DCCGetDNSRequestData(string shopperId,
                                            string sourceUrl,
                                            string orderId,
                                            string pathway,
                                            int pageCount,
                                            int privateLabelID,
                                            bool isManagerOrigin,
                                            string domainName)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _domainName = domainName;
      _privateLabelID = privateLabelID;
      _isManagerOrigin = isManagerOrigin;
      _type = null;
      RequestTimeout = _requestTimeout;
    }

    public String DomainName
    {
      get { return _domainName; }
    }

    public int PrivateLabelID
    {
      get { return _privateLabelID; }
    }

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

    public String Type
    {
      get { return _type; }
      set { _type = value; }
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("DCCGetDomainByShopper is not a cacheable request.");
    }

    #endregion
  }
}
