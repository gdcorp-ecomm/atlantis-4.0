using System;
using Atlantis.Framework.Testing.MockHttpContext;

namespace Atlantis.Framework.Providers.Localization.Tests.Mocks
{
  public class MockCustomHttpRequest : MockHttpRequest
  {
    public MockCustomHttpRequest(string requestUrl, string httpVerbName = "GET", string applicationPath = null) : base(requestUrl)
    {
      _httpVerbName = httpVerbName;
      _appPath = applicationPath;
    }

    public MockCustomHttpRequest(Uri requestUri, string httpVerbName = "GET", string applicationPath = null)
      : base(requestUri)
    {
      _httpVerbName = httpVerbName;
      _appPath = applicationPath;
    }

    private string _httpVerbName = "GET";
    public override string GetHttpVerbName()
    {
      return _httpVerbName;
    }

    private string _appPath;
    public override string GetAppPath()
    {
      return _appPath;
    }
  }
}
