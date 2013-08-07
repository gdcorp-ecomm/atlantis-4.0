using Atlantis.Framework.Interface;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Atlantis.Framework.Providers.CDSContent
{
  internal class ContentDocument : CDSDocument
  {
    public const int ContentVersionRequestType = 687;

    public ContentDocument(IProviderContainer container, string rawPath)
    {
      RawPath = rawPath;
      ProcessedPath = RawPath;

      if (HttpContext.Current != null)
      {
        var queryString = HttpContext.Current.Request.QueryString;
        var docId = queryString["version"]; //modified the query string param doc id indicates a document and not a version. version is the correct terminology
        var qsDate = queryString["activedate"];

        ISiteContext siteContext = container.Resolve<ISiteContext>();
        DateTime activeDate;
        if ((DateTime.TryParse(qsDate, out activeDate) || IsValidContentId(docId)) && siteContext.IsRequestInternal)
        {
          ByPassDataCache = true;
          var queryParams = new NameValueCollection();
          if (activeDate != default(DateTime))
          {
            queryParams.Add("activedate", activeDate.ToString("O"));
          }
          if (IsValidContentId(docId))
          {
            queryParams.Add("docid", docId);
          }
          if (queryParams.Count > 0)
          {
            string appendChar = ProcessedPath.Contains("?") ? "&" : "?";
            ProcessedPath += string.Concat(appendChar, ToQueryString(queryParams));
          }
        }
      }
    }

    public override int EngineRequestId
    {
      get { return ContentVersionRequestType; }
    }
  }
}
