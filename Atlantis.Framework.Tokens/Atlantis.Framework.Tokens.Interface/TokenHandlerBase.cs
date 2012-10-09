using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Tokens.Interface
{
  public abstract class TokenHandlerBase : ITokenHandler
  {
    public abstract string TokenKey { get; }
    public abstract TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container);
    public abstract IToken CreateToken(string tokenData, string fullTokenString);
  }
}
