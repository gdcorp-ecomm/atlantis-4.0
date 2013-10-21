using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Links.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using System;
using System.Collections.Specialized;


namespace Atlantis.Framework.Providers.Sso
{
  internal class LocalizeUrl
  {
    Lazy<ISiteContext> _siteContext;
    private Lazy<ILinkProvider> _links;
   IProviderContainer _container;

    public LocalizeUrl(IProviderContainer container)
    {
      _container = container;
      _siteContext = new Lazy<ISiteContext>(() => container.Resolve<ISiteContext>());
      _links = new Lazy<ILinkProvider>(() => container.Resolve<ILinkProvider>());
    }

    public string GetLocalizedUrl(string urlToLocalize, NameValueCollection parms)
    {
      string localizedUrl = urlToLocalize;

      try
      {
        Uri uri = new Uri(urlToLocalize);
        string host = uri.Host;

        var request = new LinkInfoRequestData(_siteContext.Value.ContextId);
        var response = (LinkInfoResponseData)DataCache.DataCache.GetProcessRequest(request, SsoProviderEngineRequests.Links);
        if (response.LinkTypesByBaseUrl.ContainsKey(host))
        {
          string linkType = response.LinkTypesByBaseUrl[host];
          bool isSecure = uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);
          string localizedHost = _links.Value.GetUrl(linkType, uri.LocalPath, QueryParamMode.CommonParameters, isSecure, parms);

          localizedUrl = string.Concat(localizedHost, uri.Query.Replace("?", "&"));
        }
        else
        {
          localizedUrl = AddParamsToNonLinkProviderUrl(parms, urlToLocalize);
        }
      }
      catch (Exception ex)
      {
        localizedUrl = AddParamsToNonLinkProviderUrl(parms, urlToLocalize);

        string errorMessage = "Error getting localizedURL. urlToLocalize:: " + urlToLocalize;
        AtlantisException aex = new AtlantisException("LocalizeUrl::GetLocalizedUrl", 0, errorMessage, ex.StackTrace);
        Engine.Engine.LogAtlantisException(aex);
      }

      return localizedUrl;
    }

    private string AddParamsToNonLinkProviderUrl(NameValueCollection parms, string urlToLocalize)
    {
      string queryStringAppender = urlToLocalize.Contains("?") ? "&" : "?";
      var convertedParams = _links.Value.GetUrlArguments(parms).Replace("?", "");
      return string.Concat(urlToLocalize, queryStringAppender, convertedParams);
    }
  }


}
