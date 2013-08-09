using Atlantis.Framework.Interface;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Atlantis.Framework.Providers.CDSContent
{
  internal abstract class CDSDocument
  {
    protected IProviderContainer Container { get; set; }
    protected string RawPath { get; set; }
    protected string ProcessedPath { get; private set; }
    protected bool ByPassDataCache { get; private set; }

    protected void AddDocIdParam(string queryStringParamName)
    {
      ProcessedPath = RawPath;

      if (HttpContext.Current != null)
      {
        var queryString = HttpContext.Current.Request.QueryString;
        var docId = queryString[queryStringParamName];
        ISiteContext siteContext = Container.Resolve<ISiteContext>();
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
  }
}
