using System;
using System.Net.Cache;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GoogleGetToken.Interface
{
  public class GoogleGetTokenRequestData : RequestData
  {
    public GoogleGetTokenRequestData(string shopperId,
                                     string sourceUrl,
                                     string orderId,
                                     string pathway,
                                     int pageCount,
                                     string oAuthToken,
            string uri_redirect)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      CacheLevel = RequestCacheLevel.BypassCache;
      this.oAuthToken = oAuthToken;
      this.UriRedirect = uri_redirect;
      RequestTimeout = TimeSpan.FromSeconds(20);
    }

    public string oAuthToken { get; private set; }
    public string UriRedirect { get; private set; }
    
    public RequestCacheLevel CacheLevel { get; set; }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      var data = Encoding.UTF8.GetBytes(string.Concat(oAuthToken,UriRedirect));

      var hash = md5.ComputeHash(data);
      var result = Encoding.UTF8.GetString(hash);
      return result;
    }
  }
}