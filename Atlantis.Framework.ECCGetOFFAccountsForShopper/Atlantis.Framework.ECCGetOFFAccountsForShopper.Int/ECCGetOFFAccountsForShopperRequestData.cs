using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetOFFAccountsForShopper.Interface
{
  public class ECCGetOFFAccountsForShopperRequestData : RequestData 
  {
    public ECCGetOFFAccountsForShopperRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string username, bool activeOnly, string emailAddress, int privateLabelId) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Username = username;
      ActiveOnly = activeOnly;
      EmailAddress = emailAddress;
      PrivateLabelId = privateLabelId;
    }

    public int PrivateLabelId { get; set; }

    public string EmailAddress { get; set; }

    public string Username { get; set; }

    public bool ActiveOnly { get; set; }
   
    public override string GetCacheMD5()
    {
      throw new Exception("ECCGetOFFAccountsForShopper is not a cacheable request.");
    }

  }
}
