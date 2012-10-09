using Atlantis.Framework.Tokens.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.TokenHandlers.Tests
{
  public class TestDynamicToken1 : SimpleTokenHandlerBase
  {
    public override string TokenKey
    {
      get { return "testdynamic1"; }
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, Framework.Interface.IProviderContainer container)
    {
      foreach (var token in tokens)
      {
        SimpleToken simple = token as SimpleToken;
        if (simple != null)
        {
          token.TokenResult = "Test Dynamic " + simple.TokenData;
        }
      }
      return TokenEvaluationResult.Success;
    }
  }
}
