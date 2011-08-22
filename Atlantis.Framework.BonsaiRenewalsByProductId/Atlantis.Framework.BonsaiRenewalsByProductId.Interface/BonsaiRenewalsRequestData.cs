using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiRenewalsByProductId.Interface
{
  public class BonsaiRenewalsRequestData : RequestData
  {
    public int UnifiedProductId { get; private set; }
    public int PrivateLabelId { get; private set; }

    public BonsaiRenewalsRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int unifiedProductId, int privateLabelId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      UnifiedProductId = unifiedProductId;
      PrivateLabelId = privateLabelId;
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public override string GetCacheMD5()
    {
      MD5 hashProvider = new MD5CryptoServiceProvider();
      hashProvider.Initialize();

      byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(string.Join(":", UnifiedProductId.ToString(), PrivateLabelId.ToString()));
      byte[] md5Bytes = hashProvider.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0).Replace("-", string.Empty);
      return sValue; 
    }
  }
}
