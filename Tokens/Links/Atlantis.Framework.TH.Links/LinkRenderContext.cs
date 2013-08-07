using System.Web;

using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Links
{
  internal class LinkRenderContext
  {
    ISiteContext _siteContext;
    ILinkProvider _linkProvider;

    internal LinkRenderContext(IProviderContainer container)
    {
      _siteContext = container.Resolve<ISiteContext>();
      _linkProvider = container.Resolve<ILinkProvider>();
    }

    internal bool RenderToken(IToken token)
    {
      bool result = true;

      LinkToken linkToken = token as LinkToken;
      if (linkToken != null)
      {
        switch (linkToken.RenderType.ToLowerInvariant())
        {
          case "imageroot":
          case "cssroot":
          case "javascriptroot":
            result = this.GetNamedRoot(linkToken);
            break;
          case "relative":
            this.GetRelativeUrl(linkToken);
            break;
          case "external":
            this.GetExternalUrl(linkToken);
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
        linkToken.TokenError = "Cannot convert IToken to LinkToken";
        linkToken.TokenResult = string.Empty;
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
          namedRoot = _linkProvider.ImageRoot;
          break;
        case "cssroot":
          namedRoot = _linkProvider.CssRoot;
          break;
        case "javascriptroot":
          namedRoot = _linkProvider.JavascriptRoot;
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
      string relativeUrl = string.Empty;

      if (token.Params == null)
      {
        relativeUrl = _linkProvider.GetRelativeUrl(token.Path, token.ParamMode);
      }
      else
      {
        relativeUrl = _linkProvider.GetRelativeUrl(token.Path, token.ParamMode, token.Params);
      }

      if (!string.IsNullOrEmpty(relativeUrl))
      {
        result = true;
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
          relativeUrl = _linkProvider.GetUrl(token.LinkType, token.Path, token.ParamMode, token.Secure.Value);
        }
        else
        {
          relativeUrl = _linkProvider.GetUrl(token.LinkType, token.Path, token.ParamMode, token.Secure.Value, token.Params);
        }
      }
      else
      {
        if (HttpContext.Current != null)
        {
          if (token.Params == null)
          {
            relativeUrl = _linkProvider.GetUrl(token.LinkType, token.Path, token.ParamMode, HttpContext.Current.Request.IsSecureConnection);
          }
          else
          {
            relativeUrl = _linkProvider.GetUrl(token.LinkType, token.Path, token.ParamMode, HttpContext.Current.Request.IsSecureConnection, token.Params);
          }
        }
      }


      if (!string.IsNullOrEmpty(relativeUrl))
      {
        result = true;
      }

      token.TokenResult = relativeUrl;
      return result;
    }
  }
}
