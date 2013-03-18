using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MSAGetFolderList.Interface
{
  public class MSAGetFolderListRequestData : RequestData
  {
    public string BaseUrl { get; set; }
    public string Hash { get; set; }
    public string ApiKey { get; set; }

    public MSAGetFolderListRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string baseUrl, string hash, string apiKey)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(10);
      BaseUrl = baseUrl;
      Hash = hash;
      ApiKey = apiKey;
    }

    public override string GetCacheMD5()
    {
      return string.Empty;
    }
  }
}
