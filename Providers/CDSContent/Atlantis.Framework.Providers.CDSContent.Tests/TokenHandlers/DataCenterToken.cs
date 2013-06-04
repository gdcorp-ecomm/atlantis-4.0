using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.Providers.CDSContent.Tests
{
  public class DataCenterToken : SimpleTokenHandlerBase
  {
    public override string TokenKey
    {
      get { return "dataCenter"; }
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      foreach (var token in tokens)
      {
        SimpleToken dataCenterToken = token as SimpleToken;
        if (dataCenterToken != null)
        {
          token.TokenResult = "Asia Pacific";
        }
      }

      return TokenEvaluationResult.Success;
    }
  }
}
