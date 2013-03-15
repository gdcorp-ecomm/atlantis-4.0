using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MSALoginUser.Interface
{
  public class MSALoginUserRequestData : RequestData
  {
    public string Email { get; set; }
    public string Password { get; set; }
    public string ApiKey { get; set; }

    public MSALoginUserRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string email, string password, string apiKey)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(10);
      Email = email;
      Password = password;
      ApiKey = apiKey;
    }

    public override string GetCacheMD5()
    {
      return string.Empty;
    }
  }

}
