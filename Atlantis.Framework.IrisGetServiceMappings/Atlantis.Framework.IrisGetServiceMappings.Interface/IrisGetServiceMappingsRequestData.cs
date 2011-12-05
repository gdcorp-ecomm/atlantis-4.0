using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.IrisGetServiceMappings.Interface
{
  public class IrisGetServiceMappingsRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(20);
    public IrisGetServiceMappingsRequestData(string shopperId,
                                             string sourceUrl,
                                             string orderId,
                                             string pathway,
                                             int pageCount,
                                             int resellerId)
      : this(shopperId, sourceUrl, orderId, pathway, pageCount, resellerId, _defaultRequestTimeout, true)
    {
    }

    public IrisGetServiceMappingsRequestData(string shopperId,
                                             string sourceUrl,
                                             string orderId,
                                             string pathway,
                                             int pageCount,
                                             int resellerId,
                                             TimeSpan requestTimeout,
                                             bool parseResponse)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ResellerId = resellerId;
      RequestTimeout = requestTimeout;
      ParseResponse = parseResponse;
    }

    public int ResellerId { get; set; }
    public bool ParseResponse { get; set; }

    public override string GetCacheMD5()
    {
      var sb = new StringBuilder("IrisGetServiceMappings", 500).Append("|").Append("plid=").Append(ResellerId);
      string key = sb.ToString();

      MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
      md5Provider.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
      byte[] md5Bytes = md5Provider.ComputeHash(stringBytes);
      return BitConverter.ToString(md5Bytes).Replace("-", string.Empty);
    }


  }
}
