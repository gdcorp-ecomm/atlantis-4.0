using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MSAGetMessage.Interface
{
  public class MSAGetMessageRequestData : RequestData
  {
    public string BaseUrl { get; set; }
    public string Hash { get; set; }
    public string ApiKey { get; set; }
    public double HeaderNum { get; set; }
    public string ExtendedHeader { get; set; }
    public string RestrictedKey { get; set; }

    public MSAGetMessageRequestData(string shopperId, string sourceUrl, string orderId,
                                        string pathway, int pageCount, string baseUrl, string hash, string apiKey,
                                        double headerNum, string extendedHeader, string restrictedKey)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(10);
      BaseUrl = baseUrl;
      Hash = hash;
      ApiKey = apiKey;
      HeaderNum = headerNum;
      ExtendedHeader = extendedHeader;
      RestrictedKey = restrictedKey;
    }

    public override string GetCacheMD5()
    {
      return string.Empty;
    }
  }
}
