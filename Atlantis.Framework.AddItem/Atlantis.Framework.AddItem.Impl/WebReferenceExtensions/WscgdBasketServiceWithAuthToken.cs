using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Atlantis.Framework.AddItem.Impl.GdBasketService;

namespace Atlantis.Framework.AddItem.Impl
{
  [ExcludeFromCodeCoverage]
  public class WscgdBasketServiceWithAuthToken : WscgdBasketService
  {
    private readonly string _authToken;

    public WscgdBasketServiceWithAuthToken(string authToken)
    {
      _authToken = authToken;
    }

    protected override WebRequest GetWebRequest(Uri uri)
    {
      var result = (HttpWebRequest)base.GetWebRequest(uri);

      if (!string.IsNullOrEmpty(_authToken))
      {
        var headerValue = "sso-jwt " + _authToken;
        result.Headers.Add("Authorization", headerValue);
      }

      return result;
    }
  }
}