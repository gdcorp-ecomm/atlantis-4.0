using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetMiniCart.Interface
{
  public class GetMiniCartRequestData : RequestData
  {
    private string _basketType = string.Empty;

    public GetMiniCartRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount)
                                  : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(3d);
    }

    public string BasketType
    {
      get { return _basketType; }
      set { _basketType = value; }
    }
    
    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("GetMiniCart is not a cacheable request.");
    }

    #endregion
  }
}
