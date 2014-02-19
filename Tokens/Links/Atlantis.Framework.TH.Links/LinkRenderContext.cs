using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Tokens.Interface;

[assembly: InternalsVisibleTo("Atlantis.Framework.TH.Links.Tests")]
namespace Atlantis.Framework.TH.Links
{
  internal class LinkRenderContext
  {
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<ILinkProvider> _linkProvider;
    private readonly Lazy<ILocalizationProvider> _localization;

    internal LinkRenderContext(IProviderContainer container)
    {
      _linkProvider = new Lazy<ILinkProvider>(container.Resolve<ILinkProvider>);
      _siteContext = new Lazy<ISiteContext>(container.Resolve<ISiteContext>);
      _localization = new Lazy<ILocalizationProvider>(container.Resolve<ILocalizationProvider>);
    }

    public ISiteContext SiteContext
    {
      get
      {
        return _siteContext.Value;
      }
    }

    internal bool RenderToken(IToken token)
    {
      bool result;

      var linkToken = token as LinkToken;

      if (linkToken != null)
      {
        switch (linkToken.RenderType.ToLowerInvariant())
        {
          case "imageroot":
          case "cssroot":
          case "javascriptroot":
            result = GetNamedRoot(linkToken);
            break;
          case "relative":
            result = GetRelativeUrl(linkToken);
            break;
          case "external":
            result = GetExternalUrl(linkToken);
            break;
          case "js":
          case "css":
            result = CreateMinifiedPath(linkToken);
            break;
          default:
            result = false;
            linkToken.TokenError = "LinkToken contains invalid RenderType";
            linkToken.TokenResult = string.Empty;
            break;
        }
      }
      else
      {
        result = false;
        token = new LinkToken(string.Empty, string.Empty, string.Empty)
        {
          TokenError = "Cannot convert IToken to LinkToken",
          TokenResult = string.Empty
        };
      }

      return result;
    }

    private bool CreateMinifiedPath(LinkToken token)
    {
      bool result = false;
      string tokenPath = token.Path;

      if (tokenPath.Contains("{"))
      {
        var dataToken = Regex.Match(token.Path, @"\{([^}]*)\}").Groups[1].Value;

        switch (dataToken)
        {
          case "marketid":
            tokenPath = tokenPath.Replace("{marketid}", _localization.Value.MarketInfo.Id.ToLower());
            break;
          case "regionsite":
            tokenPath = tokenPath.Replace("{regionsite}", _localization.Value.CountrySite.ToLower());
            break;
          case "contextid":
            tokenPath = tokenPath.Replace("{contextid}", SiteContext.ContextId.ToString());
            break;
          default:
            tokenPath = token.Path;
            break;
        }
      }

      if ("css".Equals(token.RenderType, StringComparison.OrdinalIgnoreCase) || "js".Equals(token.RenderType, StringComparison.OrdinalIgnoreCase))
      {
        string qaMinify = HttpContext.Current.Request.QueryString["qaminify"];
        bool showMinFile = ShowMinFile(qaMinify);
        string fileExt = FileExtensionUpdate(token, showMinFile);
        string rtnVal = NamedRoot(token);
        int lastDot = tokenPath.LastIndexOf('.');

        if (!string.IsNullOrEmpty(token.NameMode))
        {
          switch (token.NameMode)
          {
            case "explicit":
              token.TokenResult = String.Concat(rtnVal, tokenPath);
              break;
            case "auto":
            default:
              token.TokenResult = String.Concat(rtnVal, tokenPath.Substring(0, lastDot), fileExt);
              break;
          }

          result = true;
        }
        else if (!string.IsNullOrEmpty(tokenPath))
        {
          token.TokenResult = String.Concat(rtnVal, tokenPath.Substring(0, lastDot), fileExt);
          result = true;
        }
      }

      return result;
    }

    private bool ShowMinFile(string qaMinify)
    {
      bool showMinFile;

      switch (SiteContext.ServerLocation)
      {
        case ServerLocationType.Dev:
          showMinFile = false;
          break;
        case ServerLocationType.Test:
          showMinFile = true;
          break;
        case ServerLocationType.Ote:
          showMinFile = true;
          break;
        case ServerLocationType.Prod:
          showMinFile = true;
          break;
        case ServerLocationType.Undetermined:
        default:
          showMinFile = false;
          break;
      }

      switch (qaMinify)
      {
        case "true":
          showMinFile = true;
          break;
        case "false":
          showMinFile = false;
          break;
      }
      return showMinFile;
    }

    private string FileExtensionUpdate(LinkToken token, bool showMinFile)
    {
      string minifiedExt;
      switch (token.RenderType.ToLowerInvariant())
      {
        case "css":
          minifiedExt = showMinFile ? ".min.css" : ".css";
          break;
        case "js":
        default:
          minifiedExt = showMinFile ? ".min.js" : ".js";
          break;
      }

      return minifiedExt;
    }

    private string NamedRoot(LinkToken token)
    {
      string namedRoot = string.Empty;

      switch (token.RenderType.ToLowerInvariant())
      {
        case "imageroot":
          namedRoot = _linkProvider.Value.ImageRoot;
          break;
        case "cssroot":
        case "css":
          namedRoot = _linkProvider.Value.CssRoot;
          break;
        case "javascriptroot":
        case "js":
          namedRoot = _linkProvider.Value.JavascriptRoot;
          break;
      }
      return namedRoot;
    }

    private bool GetNamedRoot(LinkToken token)
    {
      bool result = false;

      var namedRoot = NamedRoot(token);

      if (!string.IsNullOrEmpty(namedRoot))
      {
        result = true;
      }

      token.TokenResult = namedRoot;
      return result;
    }

    private bool GetRelativeUrl(LinkToken token)
    {
      bool result = false;
      string relativeUrl;

      if (token.Params == null)
      {
        relativeUrl = _linkProvider.Value.GetRelativeUrl(token.Path, token.ParamMode);
      }
      else
      {
        relativeUrl = _linkProvider.Value.GetRelativeUrl(token.Path, token.ParamMode, token.Params);
      }

      if (!string.IsNullOrEmpty(relativeUrl))
      {
        result = true;
      }
      else
      {
        token.TokenError = "GetRelativeUrl came back empty.";
        token.TokenResult = string.Empty;
      }

      token.TokenResult = relativeUrl;

      return result;
    }

    private bool GetExternalUrl(LinkToken token)
    {
      bool result = false;
      string relativeUrl = string.Empty;

      if (token.Secure.HasValue)
      {
        if (token.Params == null)
        {
          relativeUrl = _linkProvider.Value.GetUrl(token.LinkType, token.Path, token.ParamMode, token.Secure.Value);
        }
        else
        {
          relativeUrl = _linkProvider.Value.GetUrl(token.LinkType, token.Path, token.ParamMode, token.Secure.Value, token.Params);
        }
      }
      else
      {
        if (HttpContext.Current != null)
        {
          if (token.Params == null)
          {
            relativeUrl = _linkProvider.Value.GetUrl(token.LinkType, token.Path, token.ParamMode, HttpContext.Current.Request.IsSecureConnection);
          }
          else
          {
            relativeUrl = _linkProvider.Value.GetUrl(token.LinkType, token.Path, token.ParamMode, HttpContext.Current.Request.IsSecureConnection, token.Params);
          }
        }
      }


      if (!string.IsNullOrEmpty(relativeUrl))
      {
        result = true;
      }
      else
      {
        token.TokenError = "GetExternalUrl came back empty. LinkType: " + token.LinkType;
        token.TokenResult = string.Empty;
      }

      token.TokenResult = relativeUrl;

      return result;
    }
  }
}
