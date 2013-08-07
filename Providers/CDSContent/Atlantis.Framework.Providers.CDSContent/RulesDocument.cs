using Atlantis.Framework.Interface;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Atlantis.Framework.Providers.CDSContent
{
  internal class RulesDocument : CDSDocument
  {
    public const int RoutingRulesRequestType = 696;
    private const string RulesDocFormat = "content/{0}/{1}.rule";

    public RulesDocument(IProviderContainer container, string appName, string relativePath)
    {
      RawPath = string.Format(RulesDocFormat, appName, relativePath);
      ProcessedPath = RawPath;

      if (HttpContext.Current != null)
      {
        var queryString = HttpContext.Current.Request.QueryString;
        var docId = queryString["rules"];
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
      get { return RoutingRulesRequestType; }
    }
  }
}
