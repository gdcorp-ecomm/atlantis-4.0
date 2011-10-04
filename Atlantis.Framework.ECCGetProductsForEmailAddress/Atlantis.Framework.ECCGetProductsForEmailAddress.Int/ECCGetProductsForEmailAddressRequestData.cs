using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetProductsForEmailAddress.Interface
{
  public class ECCGetProductsForEmailAddressRequestData : RequestData 
  {
    public ECCGetProductsForEmailAddressRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId, string emailAddress, string subaccount) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      EmailAddress = emailAddress;
      Subaccount = subaccount;
    }

    public int PrivateLabelId { get; set; }

    public string EmailAddress { get; set; }

    public string Subaccount { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("ECCGetProductsForEmailAddress is not a cacheable request.");
    }

    #endregion
  }
}
