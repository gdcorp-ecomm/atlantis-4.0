using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductFreeCreditsByProductId.Interface
{
  public class ProductFreeCreditsByProductIdRequestData : RequestData
  {
    #region Properties
    
    public int UnifiedProductId { get; set; }
    public int PrivateLabelId { get; set; }

    #endregion Properties

    public ProductFreeCreditsByProductIdRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int unifiedProductId
      , int privateLabelId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(2d);
      UnifiedProductId = unifiedProductId;
      PrivateLabelId = privateLabelId;
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
