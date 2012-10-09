using Atlantis.Framework.Interface;
using System.Collections.Generic;
namespace Atlantis.Framework.Tokens.Interface
{
  /// <summary>
  /// XmlTokenHandlerBase can be used if your tokendata is xml.  Its ITokens will be XmlToken objects
  /// </summary>
  public abstract class XmlTokenHandlerBase : ITokenHandler
  {
    public abstract string TokenKey { get; }
    public abstract TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container);

    public virtual IToken CreateToken(string tokenData, string fullTokenString)
    {
      return new XmlToken(TokenKey, tokenData, fullTokenString);
    }
  }
}
