using System;
using System.Web;
using Atlantis.Framework.Testing.MockHttpContext;

namespace Atlantis.Framework.Providers.ProxyContext.Tests.Mocks
{
  public class MockHttpRequestWithHostHeader : MockHttpRequest
  {

    public MockHttpRequestWithHostHeader(string requestUrl)
      : base(requestUrl)
    {
    }

    public MockHttpRequestWithHostHeader(Uri requestUri)
      : base(requestUri)
    {
    }

    public override string GetKnownRequestHeader(int index)
    {
      string headerValue;

      if (index == HeaderHost)
      {
        headerValue = base.GetUnknownRequestHeader("Host");
      }
      else
      {
        headerValue = base.GetKnownRequestHeader(index);
      }

      return headerValue;
    }

    public override string GetServerName()
    {
      if (HttpContext.Current.Items.Contains("serverName"))
      {
        return (string) HttpContext.Current.Items["serverName"];
      }
      else
      {
        return base.GetServerName();
      }
    }
  }
}
