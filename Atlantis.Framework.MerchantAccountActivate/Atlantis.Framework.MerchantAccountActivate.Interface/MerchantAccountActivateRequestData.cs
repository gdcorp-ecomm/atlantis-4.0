using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MerchantAccountActivate.Interface
{
  public class MerchantAccountActivateRequestData : RequestData
  {
    #region Properties
    public int MerchantAccountId { get; private set; }
    #endregion

    public MerchantAccountActivateRequestData(string shopperId
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
      string[] arr = {MerchantAccountId.ToString(), ShopperID};
      return BuildHashFromStrings(arr);
    }
  }
}
