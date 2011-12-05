using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSGetDomainShopper.Interface
{
  public class DBSGetDomainShopperRequestData : RequestData
  {
    private int _resourceId;
    
    public DBSGetDomainShopperRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, 
    int resourceId)
      : base (shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _resourceId = resourceId;
      RequestTimeout = TimeSpan.FromSeconds(4);
    }

    public int ResourceId
    {
      get { return _resourceId; }
      set { _resourceId = value; }
    }

    public override string GetCacheMD5()
    {
      throw new Exception("DBSGetDomainShopper is not a cacheable request.");
    }

  }
}
