using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MAFirstDataCreateMerchant.Interface
{
  public class MAFirstDataCreateMerchantRequestData : RequestData
  {
    #region Properties
    public int MerchantAccountId { get; private set; }
    public long VendorResourceId { get; private set; }
    #endregion

    public MAFirstDataCreateMerchantRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int merchantAccountId
      , long vendorResourceId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      MerchantAccountId = merchantAccountId;
      VendorResourceId = vendorResourceId;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in MAFirstDataCreateMerchantRequestData");     
    }
  }
}
