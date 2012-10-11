using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductPackagerProductGroup.Interface
{
  public class ProductPackagerProductGroupRequestData : RequestData
  {
    private string CacheKey
    {
      get
      {
        string cacheKey = string.Empty;

        if(PackageProductGroupIds != null)
        {
          cacheKey = string.Join(":", PackageProductGroupIds);
        }
        
        return cacheKey;
      }
    }

    public IList<string> PackageProductGroupIds { get; private set; }

    public ProductPackagerProductGroupRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, IList<string> packageProductGroupIds) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PackageProductGroupIds = packageProductGroupIds;
    }

    public override string GetCacheMD5()
    {
      MD5 oMd5 = new MD5CryptoServiceProvider();
      oMd5.Initialize();
      byte[] stringBytes = Encoding.ASCII.GetBytes(CacheKey);
      byte[] md5Bytes = oMd5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}