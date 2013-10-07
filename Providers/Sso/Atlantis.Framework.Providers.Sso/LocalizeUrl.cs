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
    IProviderContainer _container;

    public LocalizeUrl(IProviderContainer container)
    {
      _container = container;
      _siteContext = new Lazy<ISiteContext>(() => container.Resolve<ISiteContext>());
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

          var linkProvider = _container.Resolve<ILinkProvider>();
          string localizedHost = linkProvider.GetUrl(linkType, uri.LocalPath, QueryParamMode.CommonParameters, isSecure, parms);

          localizedUrl = string.Concat(localizedHost, uri.Query.Replace("?","&"));
        }
      }
      catch (Exception ex)
      {
        localizedUrl = urlToLocalize;

        string errorMessage = "Error getting localizedURL. urlToLocalize:: " + urlToLocalize;
        AtlantisException aex = new AtlantisException("LocalizeUrl::GetLocalizedUrl", 0, errorMessage, ex.StackTrace);
        Engine.Engine.LogAtlantisException(aex);
      }

      return localizedUrl;
    }
  }

}
