using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CatalogGetRestrictedProducts.Interface
{
  public class CatalogGetRestrictedProductsRequestData : RequestData
  {
    public CatalogGetRestrictedProductsRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      string privateLabelId) :
      base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      PrivateLabelId = privateLabelId;
    }

    public string PrivateLabelId { get; set; }

    public string HashKey
    {
      get
      {
        return PrivateLabelId;
      }
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(HashKey);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
