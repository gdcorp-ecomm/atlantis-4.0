using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductPackagerProductPackage.Interface
{
  public class ProductPackagerProductPackageRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(8);

    private string CacheKey
    {
      get
      {
        string cacheKey = string.Empty;

        if (PackageIds != null)
        {
          cacheKey = string.Join(":", PackageIds);
        }

        return cacheKey;
      }
    }

    public IList<string> PackageIds { get; private set; }

    public ProductPackagerProductPackageRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, IList<string> packageIds) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PackageIds = packageIds;
      RequestTimeout = _defaultRequestTimeout;
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
