using System.Text.RegularExpressions;
using System.Web;

namespace Atlantis.Framework.Providers.Localization.Tests.Mocks.Http
{
  public class MockHttpRequest : HttpRequestBase
  {
    public MockHttpRequest(HttpRequest request = null, string httpMethod = "GET", string virtualFolder = "") : base()
    {
      BaseRequest = request;
      _httpMethod = httpMethod;
      _applicationPath = "/" + virtualFolder.ToLower();
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
  }
}
