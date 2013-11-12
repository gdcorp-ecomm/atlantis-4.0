using Atlantis.Framework.Interface;
using Atlantis.Framework.Tokens.Interface;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Atlantis.Framework.TH.Domains
{
  public class DomainsTokenHandler : XmlTokenHandlerBase
  {
    public override string TokenKey
    {
      get
      {
        return "domains";
      }
    }

    public override IToken CreateToken(string tokenData, string fullTokenString)
    {
      var rtnVal = new XmlToken(TokenKey, tokenData, fullTokenString);
      TokenType tokenType;
      if (!ReferenceEquals(null, rtnVal.TokenData) && Enum.TryParse(rtnVal.TokenData.Name.ToString(), true, out tokenType))
      {
        switch (tokenType)
        {
          case TokenType.ICANNTlds:
            rtnVal = new GrammaticalDelimiterToken(TokenKey, tokenData, fullTokenString);
            break;
        }
      }
      return rtnVal;
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      TokenEvaluationResult returnValue = TokenEvaluationResult.Errors;

      var contextRenderer = new DomainsTokenRenderContext(container);

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
