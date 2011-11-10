using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyerProxyAccountsGet.Interface
{
  public class BuyerProxyAccountsGetRequestData : RequestData
  {
    public BuyerProxyAccountsGetRequestData(string shopperId, 
                                            string sourceURL, 
                                            string orderId, 
                                            string pathway, 
                                            int pageCount) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("BuyerProxyAccountsGetRequestData is not a cacheable request.");
    }

    #endregion

  }
}
