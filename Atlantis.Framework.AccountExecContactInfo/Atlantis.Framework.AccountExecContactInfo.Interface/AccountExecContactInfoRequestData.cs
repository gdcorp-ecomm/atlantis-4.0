using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AccountExecContactInfo.Interface
{
  public class AccountExecContactInfoRequestData : RequestData
  {
    public AccountExecContactInfoRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(ShopperID);
    }
  }
}
