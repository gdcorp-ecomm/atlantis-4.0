using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;

namespace Atlantis.Framework.Providers.Localization.Tests.Mocks.Http
{
  public class MockHttpRequest : HttpRequestBase
  {
    private NameValueCollection _headers = new NameValueCollection();

    public MockHttpRequest(HttpRequest request = null, string httpMethod = "GET", string virtualFolder = "", bool autoSetHostHeader = true) : base()
    {
      BaseRequest = request;
      _httpMethod = httpMethod;
      _applicationPath = "/" + virtualFolder.ToLower();

      if (request != null && autoSetHostHeader && string.IsNullOrEmpty(_headers["Host"]))
      {
        _headers["Host"] = request.Url.Host;
      }
    }

    private HttpRequest BaseRequest { get; set; }

    private string _httpMethod = "GET";
    public override string HttpMethod { get { return _httpMethod; } }

    private string _applicationPath;
    public override string ApplicationPath { get { return _applicationPath; } }

    public override string AppRelativeCurrentExecutionFilePath
    {
      get
      {
        Regex re = new Regex("^" + ApplicationPath + "/", RegexOptions.IgnoreCase);
        return "~" + re.Replace(BaseRequest.Path, "/", 1);
      }
    }

    public override string Path
    {
      get
      {
        { return BaseRequest.Path;}
      }
    }

    public override string RawUrl
    {
      get
      {
        return BaseRequest.RawUrl;
      }
    }

    public override System.Uri Url
    {
      get
      {
        return BaseRequest.Url;
      }
    }

    public override string[] UserLanguages
    {
      get
      {
        return BaseRequest.UserLanguages;
      }
    }

    public override System.Collections.Specialized.NameValueCollection QueryString
    {
      get
      {
        return BaseRequest.QueryString;
      }
    }

    public override HttpCookieCollection Cookies
    {
      get
      {
        return BaseRequest.Cookies;
      }
    }

    public override NameValueCollection Headers
    {
      get { return _headers; }
    }
  }
}
