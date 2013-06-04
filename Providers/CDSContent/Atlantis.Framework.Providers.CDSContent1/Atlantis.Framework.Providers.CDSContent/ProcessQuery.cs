using Atlantis.Framework.Interface;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Atlantis.Framework.Providers.CDSContent
{
  class ProcessQuery
  {
    public string Query { get; set; }
    public bool BypassCache { get; set; }

    public ProcessQuery(IProviderContainer container, string query)
    {
      Query = query;
      ISiteContext siteContext = container.Resolve<ISiteContext>();

      if (HttpContext.Current != null)
      {
        DateTime activeDate;
        var queryString = HttpContext.Current.Request.QueryString;
        var docId = queryString["version"]; //modified the query string param doc id indicates a document and not a version. version is the correct terminology
        var qsDate = queryString["activedate"];
        if ((DateTime.TryParse(qsDate, out activeDate) || IsValidContentId(docId)) && siteContext.IsRequestInternal)
        {
          BypassCache = true;
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
            string appendChar = Query.Contains("?") ? "&" : "?";
            Query += string.Concat(appendChar, ToQueryString(queryParams));
          }
        }
      }
    }

    #region Private

    private bool IsValidContentId(string text)
    {
      bool result = false;
      if (text != null)
      {
        string pattern = @"^[0-9a-fA-F]{24}$";
        result = Regex.IsMatch(text, pattern);
      }
      return result;
    }

    private string ToQueryString(NameValueCollection nvc)
    {
      return string.Join("&", nvc.AllKeys.SelectMany(key => nvc.GetValues(key).Select(value => string.Format("{0}={1}", key, value))).ToArray());
    }

    #endregion
  }
}
