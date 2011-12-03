using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CatalogGetCategoriesCache.Interface
{
  public class CatalogGetCategoriesCacheRequestData : RequestData
  {
     public CatalogGetCategoriesCacheRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      string privateLabelId, int versionNumber) :
      base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      PrivateLabelId = privateLabelId;
      VersionNumber = versionNumber;
    }

    public string PrivateLabelId { get; set; }
    public int VersionNumber { get; set; }

    public string HashKey
    {
      get
      {
        return string.Concat(PrivateLabelId,"|",VersionNumber.ToString());
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
