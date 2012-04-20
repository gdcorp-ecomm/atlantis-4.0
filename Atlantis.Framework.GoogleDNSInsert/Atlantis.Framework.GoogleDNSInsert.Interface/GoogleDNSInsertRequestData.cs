using System;
using System.Net.Cache;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GoogleDNSInsert.Interface
{
  public class GoogleDNSInsertRequestData : RequestData
  {
    public GoogleDNSInsertRequestData(string shopperId,
                                     string sourceUrl,
                                     string orderId,
                                     string pathway,
                                     int pageCount,
                                        string privateAccessToken,
            string domainName)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      CacheLevel = RequestCacheLevel.BypassCache;
      this.PrivateAccessToken = privateAccessToken;
      this.DomainName = domainName;
      RequestTimeout = TimeSpan.FromSeconds(20);
    }

    public string PrivateAccessToken { get; private set; }
    public string DomainName { get; private set; }
    
    public RequestCacheLevel CacheLevel { get; set; }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      var data = Encoding.UTF8.GetBytes(string.Concat(PrivateAccessToken, DomainName));

      var hash = md5.ComputeHash(data);
      var result = Encoding.UTF8.GetString(hash);
      return result;
    }
  }
}