using Atlantis.Framework.Interface;
using Atlantis.Framework.Tokens.Interface;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Atlantis.Framework.TH.Support
{
  public class SupportTokenHandler : XmlTokenHandlerBase
  {
    public override string TokenKey
    {
      get
      {
        return "support";
      }
    }

    public override IToken CreateToken(string tokenData, string fullTokenString)
    {
      return new SupportToken(TokenKey, tokenData, fullTokenString);
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      TokenEvaluationResult returnValue = TokenEvaluationResult.Success;

      if (!ReferenceEquals(null, tokens) && 0 < tokens.Count())
      {
        var contextRenderer = new SupportRenderContext(container);

        foreach (IToken token in tokens)
        {
          bool success = contextRenderer.RenderToken(token);
          if (!success)
          {
            returnValue = TokenEvaluationResult.Errors;
          }
        } 
      }

      return returnValue;
    }
  }
}
