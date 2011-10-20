using System;
using System.Net.Cache;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.XmlHttpFileGet.Interface
{
  public class XmlHttpFileGetRequestData : RequestData
  {
    public XmlHttpFileGetRequestData(string shopperId,
                                     string sourceUrl,
                                     string orderId,
                                     string pathway,
                                     int pageCount,
                                     string xmlUrlPath) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      CacheLevel = RequestCacheLevel.Default;
      XmlUrlPath = xmlUrlPath;
      RequestTimeout = TimeSpan.FromSeconds(20);
    }

    public string XmlUrlPath { get; private set; }
    public RequestCacheLevel CacheLevel { get; set; }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      var data = Encoding.UTF8.GetBytes(XmlUrlPath);

      var hash = md5.ComputeHash(data);
      var result = Encoding.UTF8.GetString(hash);
      return result;
    }
  }
}