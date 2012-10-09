using Atlantis.Framework.Tokens.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Tokens.Tests.Handlers
{
  public class SimpleTokenHandler : SimpleTokenHandlerBase
  {
    public override string TokenKey
    {
      get { return "testsimple"; }
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, Framework.Interface.IProviderContainer container)
    {
      foreach (var token in tokens)
      {
        SimpleToken simple = token as SimpleToken;
        if (simple != null)
        {
          token.TokenResult = simple.TokenData;
        }
      }
      return TokenEvaluationResult.Success;
    }
  }
}
