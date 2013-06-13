using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Tokens.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TH.Currency
{
  public class CurrencyPriceTokenHandler : XmlTokenHandlerBase
  {
    public override string TokenKey
    {
      get { return "currencyprice"; }
    }

    public override IToken CreateToken(string tokenData, string fullTokenString)
    {
      return new CurrencyPriceToken(TokenKey, tokenData, fullTokenString);
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      TokenEvaluationResult result = TokenEvaluationResult.Success;

      CurrencyPriceRenderContext contextRenderer = new CurrencyPriceRenderContext(container);

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
