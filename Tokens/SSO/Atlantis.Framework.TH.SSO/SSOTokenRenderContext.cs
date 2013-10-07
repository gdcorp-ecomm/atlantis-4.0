using System;
using Atlantis.Framework.Tokens.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.BasePages;
using System.Xml.Linq;
using Atlantis.Framework.DataCache;
using Atlantis.Framework.Providers.Interface.Links;
using System.Linq;
using Atlantis.Framework.Providers.Sso.Interface;

namespace Atlantis.Framework.TH.SSO
{
  public enum TokenType
  {
    SpKey,
    SpGroupName,
    LogInUrl,
    LogOutUrl
  }

  public class SSOTokenRenderContext
  {
    //private const string _BR_PREFIX = "BRSWNET";
    //private const string _GD_PREFIX = "GDSWNET";
    //private const string _SPN_PREFIX = "SPSWNET";
    //private const string _WWD_PREFIX = "WWDSWNET";

    //private Lazy<string> _clusterName = new Lazy<string>(() => string.Format("{0}CORPWEB", System.Configuration.ConfigurationManager.AppSettings["DataCenter"]));
    //private Lazy<ILinkProvider> _links;
    //private Lazy<ILocalizationProvider> _localization;
    //private Lazy<string> _serviceProviderGroupSuffix;

    private struct IdentityProviderInfo
    {
      public string SpKey { get; set; }
      public string SpGroupName { get; set; }
      public string LogInUrl { get; set; }
      public string LogOutUrl { get; set; }
    }

    public SSOTokenRenderContext(IProviderContainer providerContainer)
    {
      if (ReferenceEquals(null, providerContainer))
        throw new ArgumentNullException("providerContainer", "providerContainer is null.");

      ProviderContainer = providerContainer;

      //_localization = new Lazy<ILocalizationProvider>(() => ProviderContainer.Resolve<ILocalizationProvider>());
      //_links = new Lazy<ILinkProvider>(() => ProviderContainer.Resolve<ILinkProvider>());
      //_serviceProviderGroupSuffix = new Lazy<string>(() => _localization.Value.IsGlobalSite() ? string.Empty : string.Concat(".", _localization.Value.CountrySite.ToUpperInvariant()));
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <remarks>This should be in the SSO provider for the sales site</remarks>
    //private string ClusterName
    //{
    //  get
    //  {
    //    return _clusterName.Value;
    //  }
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <remarks>This should be in the provider for the sales site</remarks>
    //private ILinkProvider Links
    //{
    //  get { return _links.Value; }
    //}

    public IProviderContainer ProviderContainer { get; private set; }

    public bool RenderToken(IToken token)
    {
      bool returnValue = false;

      SimpleToken cast = token as SimpleToken;

      if (!ReferenceEquals(null, cast))
      {
        try
        {
          TokenType tokenType;
          if (Enum.TryParse(token.RawTokenData, true, out tokenType))
          {
            var data = GetProviderInfo();

            string result = String.Empty;
            switch (tokenType)
            {
              case TokenType.SpKey:
                result = data.SpKey;
                break;
              case TokenType.SpGroupName:
                result = data.SpGroupName;
                break;
              case TokenType.LogInUrl:
                result = data.LogInUrl;
                break;
              case TokenType.LogOutUrl:
                result = data.LogOutUrl;
                break;
            }

            token.TokenResult = result;
            returnValue = true;
          }
          else
          {
            throw new ApplicationException(String.Format("Unable to retrieve Identity Provider Information for {0}.", cast.RawTokenData));
          }
        }
        catch (Exception ex)
        {
          returnValue = false;
          token.TokenResult = string.Empty;
          token.TokenError = ex.Message;
          LogDebugMessage(ex.Message, ex.Source);
        }
      }

      return returnValue;
    }

    private IdentityProviderInfo GetProviderInfo()
    {
      IdentityProviderInfo returnValue = new IdentityProviderInfo();

      var ssoProvider = ProviderContainer.Resolve<ISsoProvider>();
      try
      {
        returnValue.SpKey = ssoProvider.SpKey;
        returnValue.SpGroupName = ssoProvider.ServiceProviderGroupName;
        returnValue.LogInUrl = ssoProvider.GetUrl(SsoUrlType.Login);
        returnValue.LogOutUrl = ssoProvider.GetUrl(SsoUrlType.Logout);
      }
      catch (Exception ex)
      {
        LogDebugMessage(ex.Message, ex.Source);
      }

      //// TODO: Change this to use the sso provider
      //string clusterName = Links.IsDebugInternal() ? "G1DWCORPWEB" : ClusterName;

      //string spGroupName = GetSpGroupName(contextId);
      //string requestXml =
      //  string.Format(
      //    "<ssoGetIdentityProviderByServer><param name=\"serviceProviderGroupName\" value=\"{0}\"/><param name=\"serverName\" value=\"{1}\"/></ssoGetIdentityProviderByServer>",
      //    spGroupName,
      //    clusterName);
      //try
      //{
      //  string data = Atlantis.Framework.DataCache.DataCache.GetCacheData(requestXml);
      //  if (string.IsNullOrEmpty(data))
      //  {
      //    throw new Exception(string.Format("Identity Provider lookup returned a null or empty string. Request: '{0}'", requestXml));
      //  }
      //  XElement response = XElement.Parse(data);
      //  if (response.HasElements && response.Elements("item").Any())
      //  {
      //    XElement responseElement = response.Element("item");
      //    returnValue.SpKey = responseElement.Attribute("spkey").Value;
      //    returnValue.LogInUrl = responseElement.Attribute("loginURL").Value;
      //    returnValue.LogOutUrl = responseElement.Attribute("logoutURL").Value;
      //    returnValue.SpGroupName = spGroupName;
      //  }
      //  else
      //  {
      //    throw new Exception(string.Format("Error reading Identity Provider lookup response. Request: '{0}', Response: '{1}'", requestXml, data));
      //  }
      //}
      //catch (Exception ex)
      //{
      //  LogDebugMessage(ex.Message, ex.Source);
      //}

      return returnValue;
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <remarks>This should be in the SSO provider for the sales site</remarks>
    //private string GetSpGroupName(int contextId)
    //{
    //  string returnValue = string.Empty;

    //  switch (contextId)
    //  {
    //    case ContextIds.GoDaddy:
    //      returnValue = _GD_PREFIX;
    //      break;
    //    case ContextIds.BlueRazor:
    //      returnValue = _BR_PREFIX;
    //      break;
    //    case ContextIds.WildWestDomains:
    //      returnValue = _WWD_PREFIX;
    //      break;
    //    default:
    //      returnValue = _SPN_PREFIX;
    //      break;
    //  }
    //  string isInternal = Links.IsDebugInternal() ? "-DEBUG64" : string.Empty;
    //  returnValue = string.Format("{0}{1}{2}", returnValue, _serviceProviderGroupSuffix.Value, isInternal).Trim();

    //  return returnValue;
    //}

    private void LogDebugMessage(string message, string errorSource)
    {
      if (this.ProviderContainer.CanResolve<IDebugContext>())
      {
        this.ProviderContainer.Resolve<IDebugContext>().LogDebugTrackingData(errorSource, message);
      }
    }

  }
}
