using Atlantis.Framework.Interface;
using System.Collections.Generic;
namespace Atlantis.Framework.Tokens.Interface
{
  /// <summary>
  /// SimpleTokenHandlerBase can be used if your tokendata is a simple string.  Its ITokens will be SimpleToken objects
  /// </summary>
  public abstract class SimpleTokenHandlerBase : ITokenHandler
  {
    public abstract string TokenKey { get; }
    public abstract TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container);

    public virtual IToken CreateToken(string tokenData, string fullTokenString)
    {
      return new SimpleToken(TokenKey, tokenData, fullTokenString);
    }
  }
}
