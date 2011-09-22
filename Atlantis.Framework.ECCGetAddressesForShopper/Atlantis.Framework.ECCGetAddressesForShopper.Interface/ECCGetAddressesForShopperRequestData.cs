using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetAddressesForShopper.Interface
{
  public class ECCGetAddressesForShopperRequestData : RequestData
  {

    public int PrivateLabelId { get; private set; }
    public int EmailType { get; private set; }
    public bool Active { get; private set; }
    public string SubAccount { get; private set; }

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);
    
    public ECCGetAddressesForShopperRequestData(string shopperId, 
      int privateLabelId,
      int emailType,
      bool active,
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      base.RequestTimeout = _requestTimeout;
      PrivateLabelId = privateLabelId;
      EmailType = emailType;
      Active = active;
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("ECCGetAddressesForShopper is not a cacheable request");
    }

    #endregion
  }
}
