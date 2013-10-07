using System;
using Atlantis.Framework.Tokens.Interface;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TH.SSO
{
  public class SSOTokenHandler : SimpleTokenHandlerBase
  {
    public override string TokenKey
    {
      get { return "sso"; }
    }
    
    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      TokenEvaluationResult returnValue = TokenEvaluationResult.Errors;

      var contextRenderer = new SSOTokenRenderContext(container);

      if (!ReferenceEquals(null, tokens))
      {
        returnValue = TokenEvaluationResult.Success;
        foreach (IToken token in tokens)
        {
          if (!contextRenderer.RenderToken(token))
          {
            returnValue = TokenEvaluationResult.Errors;
          }
        } 
      }

      return returnValue;
    }

  }
}
