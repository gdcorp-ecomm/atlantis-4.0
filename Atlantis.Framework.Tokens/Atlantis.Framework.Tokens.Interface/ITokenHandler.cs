using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Atlantis.Framework.Tokens.Interface
{
  [InheritedExport]
  public interface ITokenHandler
  {
    string TokenKey { get; }
    TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container);
    IToken CreateToken(string tokenData, string fullTokenString);
  }
}
