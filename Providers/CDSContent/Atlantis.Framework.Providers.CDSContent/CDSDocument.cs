using Atlantis.Framework.Interface;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.CDSContent
{
  internal abstract class CDSDocument
  {
    const string CLIENT_SPOOF_PARAM_NAME = "version";
    const string SERVICE_SPOOF_PARAM_NAME = "docid";
    const string PATH_PREFIX = "content/";

    protected IProviderContainer Container { get; set; }
    protected string DefaultContentPath { get; set; }
    protected string ContentPath { get; private set; }
    protected bool ByPassDataCache { get; private set; }

    protected void SetContentPath()
    {
      ContentPath = DefaultContentPath;
      ISiteContext siteContext = Container.Resolve<ISiteContext>();

      if (siteContext.IsRequestInternal && HttpContext.Current != null)
      {
        string list = HttpContext.Current.Request.Params[CLIENT_SPOOF_PARAM_NAME];
        string rawPathWithoutPrefix = DefaultContentPath.Replace(PATH_PREFIX, string.Empty);
        string versionId = GetSpoofedVersionId(rawPathWithoutPrefix, list);
        if (IsValidContentId(versionId))
        {
          ByPassDataCache = true;
          var queryParams = new NameValueCollection();
          queryParams.Add(SERVICE_SPOOF_PARAM_NAME, versionId);
          string appendChar = ContentPath.Contains("?") ? "&" : "?";
          ContentPath += string.Concat(appendChar, ToQueryString(queryParams));
        }
      }
    }

    private string GetSpoofedVersionId(string key, string list)
    {
      string value = null;

      if (!string.IsNullOrEmpty(list) && list.Contains(key))
      {
        string[] pairs = list.Split(new char[] { ',' });

        if (pairs.Length > 0)
        {
          //Gets the value in the first pair where the key matches
          //Example query string: version=sales/hosting/web-hosting|12345678,sales/_shared/global-css|87654321
          value = (from pair in pairs
                   let pairArray = pair.Split(new char[] { '|' })
                   where pairArray.Length == 2
                   && pairArray[0] == key
                   select pairArray[1]).FirstOrDefault();
        }
      }
      return value;
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
