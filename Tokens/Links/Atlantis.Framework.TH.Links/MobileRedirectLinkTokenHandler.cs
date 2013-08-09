using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Links
{
  public class MobileRedirectLinkTokenHandler : XmlTokenHandlerBase
  {
    public override string TokenKey
    {
      get { return "mobileRedirectLink"; }
    }

    public override IToken CreateToken(string tokenData, string fullTokenString)
    {
      return new MobileRedirectLinkToken(TokenKey, tokenData, fullTokenString);
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      var result = TokenEvaluationResult.Success;

      var contextRenderer = new MobileRedirectLinkRenderContext(container);

      foreach (IToken token in tokens)
      {
        bool success = contextRenderer.RenderToken(token);
        if (!success)
        {
          result = TokenEvaluationResult.Errors;
        }
      }

      return result;
    }
  }
}
