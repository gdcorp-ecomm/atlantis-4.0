using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DBSSellerNotInterested.Interface
{
  public class DBSSellerNotInterestedRequestData : RequestData
  {
    private int _resourceId;

    public DBSSellerNotInterestedRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount,
    int resourceId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
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
      throw new Exception("DBSSellerNotInterested is not a cacheable request.");
    }

  }
}
