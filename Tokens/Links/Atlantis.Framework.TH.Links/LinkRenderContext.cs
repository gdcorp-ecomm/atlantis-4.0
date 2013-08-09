using System;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Links
{
  internal class LinkRenderContext
  {
    private Lazy<ILinkProvider> _linkProvider;

    internal LinkRenderContext(IProviderContainer container)
    {
      _linkProvider = new Lazy<ILinkProvider>(container.Resolve<ILinkProvider>);
    }

    internal bool RenderToken(IToken token)
    {
      bool result;

      LinkToken linkToken = token as LinkToken;

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
        token.TokenError = "Cannot convert IToken to LinkToken";
        token.TokenResult = string.Empty;
      }

      return result;
    }

    private bool GetNamedRoot(LinkToken token)
    {
      bool result = false;

      string namedRoot = string.Empty;

      switch (token.RenderType.ToLowerInvariant())
      {
        case "imageroot":
          namedRoot = _linkProvider.Value.ImageRoot;
          break;
        case "cssroot":
          namedRoot = _linkProvider.Value.CssRoot;
          break;
        case "javascriptroot":
          namedRoot = _linkProvider.Value.JavascriptRoot;
          break;
      }

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
