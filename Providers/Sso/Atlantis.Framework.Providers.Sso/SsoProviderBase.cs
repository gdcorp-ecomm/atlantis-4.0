using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Sso.Interface;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.Sso
{
  public abstract class SsoProviderBase : ProviderBase, ISsoProvider
  {
    #region Properties
    private string _loginUrl = string.Empty;
    private string _logoutUrl = string.Empty;
    private string _spkey = string.Empty;

    protected Lazy<ILinkProvider> _links;
    protected Lazy<ISiteContext> _siteContext;
    protected Lazy<IShopperContext> _shopperContext;

    LocalizeUrl _localizeUrl;
    private LocalizeUrl LocalizeUrl
    {
      get { return _localizeUrl ?? (_localizeUrl = new LocalizeUrl(this.Container)); }
    }

    private bool? _httpRequestExists;
    protected bool HttpRequestExists
    {
      get
      {
        if (!_httpRequestExists.HasValue)
        {
          _httpRequestExists = HttpContext.Current != null && HttpContext.Current.Request != null;
        }

        return _httpRequestExists.Value;
      }
    }

    private bool? _sessionExists;
    protected bool SessionExists
    {
      get
      {
        if (!_sessionExists.HasValue)
        {
          _sessionExists = HttpRequestExists && HttpContext.Current.Session != null;
        }

        return _sessionExists.Value;
      }
    }

    private string _failedLoginCountKey = null;
    protected string FailedLoginCountKey
    {
      get
      {
        return _failedLoginCountKey ?? (_failedLoginCountKey = "SsoProvider.FailLoginCount");
      }
    }

    public abstract string ServiceProviderGroupName { get; }
    public abstract string ServerOrClusterName { get; }

    public string SpKey
    {
      get
      {
        return _spkey;
      }
    }

    #endregion

    public SsoProviderBase(IProviderContainer container)
      : base(container)
    {
      _links = new Lazy<ILinkProvider>(() => container.Resolve<ILinkProvider>());
      _siteContext = new Lazy<ISiteContext>(() => container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => container.Resolve<IShopperContext>());
      InitializeSsoData();
    }

    public abstract bool ParseArtifact(string artifact, out string shopperId);
    public abstract bool ParseArtifact(string artifact, out string shopperId, out int failureCount);

    public string GetUrl(SsoUrlType ssoUrlType)
    {
      return GetUrl(ssoUrlType, new NameValueCollection());
    }

    public string GetUrl(SsoUrlType ssoUrlType, NameValueCollection additionalParams)
    {
      string urlToReturn = string.Empty;
      additionalParams["spkey"] = SpKey;

      switch (ssoUrlType)
      {
        case SsoUrlType.Login:
          urlToReturn = LocalizeUrl.GetLocalizedUrl(_loginUrl, additionalParams);
          break;
        case SsoUrlType.Logout:
          urlToReturn = LocalizeUrl.GetLocalizedUrl(_logoutUrl, additionalParams);
          break;
        case SsoUrlType.KeepAlive:
          additionalParams.Remove("spkey");
          urlToReturn = _links.Value.GetUrl("SSOURL", "keepalive.aspx", QueryParamMode.CommonParameters, true, additionalParams);
          break;
      }

      return urlToReturn;
    }

    protected virtual int AddFailedLoginTransaction()
    {
      int count = 1;

      if (SessionExists)
      {
        object existingCountObject = HttpContext.Current.Session[FailedLoginCountKey];
        if ((existingCountObject != null) && (existingCountObject is int))
        {
          count = count + (int)existingCountObject;
        }

        HttpContext.Current.Session[FailedLoginCountKey] = count;
      }

      return count;
    }

    protected virtual void ResetFailedLoginTransaction()
    {
      if (SessionExists)
      {
        object existingCountObject = HttpContext.Current.Session[FailedLoginCountKey];
        if (existingCountObject != null)
        {
          HttpContext.Current.Session[FailedLoginCountKey] = 0;
        }
      }

    }

    private void InitializeSsoData()
    {
      string xmlFormatString = "<ssoGetIdentityProviderByServer><param name=\"serviceProviderGroupName\" value=\"{0}\"/><param name=\"serverName\" value=\"{1}\"/></ssoGetIdentityProviderByServer>";
      string requestXml = string.Format(xmlFormatString, ServiceProviderGroupName, ServerOrClusterName);
      string returnXmlData = string.Empty;

      try
      {
        returnXmlData = DataCache.DataCache.GetCacheData(requestXml);
        var response = XElement.Parse(returnXmlData);
        var item = response.Element("item");
        _spkey = item.Attribute("spkey").Value;
        _loginUrl = item.Attribute("loginURL").Value;
        _logoutUrl = item.Attribute("logoutURL").Value;
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("Error reading Identity Provider lookup response. Request: '{0}', Response: '{1}'", requestXml, returnXmlData);
        AtlantisException aex = new AtlantisException("ClusterSsoProvider::InitializeSsoData", 0, errorMessage, ex.StackTrace);
        Engine.Engine.LogAtlantisException(aex);
      }
    }
  }
}
