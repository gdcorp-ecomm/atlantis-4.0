using System;
using System.Security.Cryptography;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ResourceBillingInfo.Interface
{
  public class ResourceBillingInfoRequestData : RequestData
  {
    public int? BillingResourceId { get; set; }

    public ResourceBillingInfoRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  int? billingResourceId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      BillingResourceId = billingResourceId;
      RequestTimeout = TimeSpan.FromSeconds(5d);
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(string.Format("{0}-{1}", ShopperID, (BillingResourceId.HasValue ? BillingResourceId.Value.ToString() : "GetAll")));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
