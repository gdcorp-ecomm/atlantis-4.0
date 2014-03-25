using Atlantis.Framework.Interface;
using Atlantis.Framework.Tokens.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.TH.Localization
{
  public class LocalizationTokenHandler : XmlTokenHandlerBase
  {
    public override string TokenKey
    {
      get { return "localization"; }
    }

    public override IToken CreateToken(string tokenData, string fullTokenString)
    {
      return new LocalizationToken(TokenKey, tokenData, fullTokenString);
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      TokenEvaluationResult result = TokenEvaluationResult.Success;

      LocalizationRenderContext contextRenderer = new LocalizationRenderContext(container);

      foreach (IToken token in tokens)
      {
        bool success = contextRenderer.RenderToken(token);
        if (!success)
        {
          result = TokenEvaluationResult.Errors;
        }
      }

      return result;
    }
  }
}

