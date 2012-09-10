using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MAFirstDataCreateApplication.Interface
{
  public class MAFirstDataCreateApplicationRequestData : RequestData
  {
    #region Properties
    public int MerchantAccountId { get; private set; }
    #endregion

    public MAFirstDataCreateApplicationRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int merchantAccountId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      MerchantAccountId = merchantAccountId;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in MAFirstDataCreateApplicationRequestData");     
    }
  }
}
