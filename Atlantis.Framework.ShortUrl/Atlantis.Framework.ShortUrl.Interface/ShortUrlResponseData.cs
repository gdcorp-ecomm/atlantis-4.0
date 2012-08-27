using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShortUrl.Interface
{
  public class ShortUrlResponseData : IResponseData
  {
    private AtlantisException _atlEx;

    public string ShortUrl { get; private set; }

    public string OriginalUrl { get; private set; }

    public bool IsSuccess { get; private set; }

    public ShortUrlResponseData(string shortUrl, ShortUrlRequestData requestData)
    {
      ShortUrl = shortUrl;
      OriginalUrl = requestData.Url;
      IsSuccess = !string.IsNullOrEmpty(ShortUrl) && Uri.IsWellFormedUriString(ShortUrl, UriKind.Absolute);
    }

    public ShortUrlResponseData(Exception ex, ShortUrlRequestData requestData)
    {
      ShortUrl = string.Empty;
      OriginalUrl = requestData.Url;
      _atlEx = new AtlantisException(requestData, ex.StackTrace, ex.Message + " | " + ex.StackTrace, string.Empty);
      IsSuccess = false;
    }

    public string ToXML()
    {
      return ShortUrl;
    }

    public AtlantisException GetException()
    {
      return _atlEx;
    }
  }
}
