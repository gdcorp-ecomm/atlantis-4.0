using Atlantis.Framework.Interface;
using System.Collections.Specialized;
using System.Web;

namespace Atlantis.Framework.Providers.CDSContent
{
  internal class WhitelistDocument : CDSDocument
  {
    private const string WhiteListFormat = "content/{0}/whitelist";
    public const int UrlWhitelistRequestType = 688;

    public WhitelistDocument(IProviderContainer container, string appName)
    {
      RawPath = string.Format(WhiteListFormat, appName);
      ProcessedPath = RawPath;

      if (HttpContext.Current != null)
      {
        var queryString = HttpContext.Current.Request.QueryString;
        var docId = queryString["whitelist"];
        ISiteContext siteContext = container.Resolve<ISiteContext>();
        if (IsValidContentId(docId) && siteContext.IsRequestInternal)
        {
          ByPassDataCache = true;
          var queryParams = new NameValueCollection();
          queryParams.Add("docid", docId);
          string appendChar = ProcessedPath.Contains("?") ? "&" : "?";
          ProcessedPath += string.Concat(appendChar, ToQueryString(queryParams));
        }
      }
    }

    public override int EngineRequestId
    {
      get { return UrlWhitelistRequestType; }
    }
  }
}
