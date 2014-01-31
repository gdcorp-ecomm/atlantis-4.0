using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiRenewalsByProductId.Interface
{
  public class BonsaiProductRenewalRequestData : RequestData
  {
    public int UnifiedProductId { get; private set; }
    public int PrivateLabelId { get; private set; }

    public BonsaiProductRenewalRequestData(int unifiedProductId, int privateLabelId)
    {
      UnifiedProductId = unifiedProductId;
      PrivateLabelId = privateLabelId;
      RequestTimeout = TimeSpan.FromSeconds(20d);
    }

    public override string GetCacheMD5()
    {
      MD5 hashProvider = new MD5CryptoServiceProvider();
      hashProvider.Initialize();

      var stringBytes = System.Text.Encoding.ASCII.GetBytes(string.Join(":", UnifiedProductId.ToString(), PrivateLabelId.ToString()));
      var md5Bytes = hashProvider.ComputeHash(stringBytes);
      var sValue = BitConverter.ToString(md5Bytes, 0).Replace("-", string.Empty);
      return sValue; 
    }
  }
}
