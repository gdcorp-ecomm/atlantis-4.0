using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Brand.Interface;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Brand
{
  public class ProductLineTokenHandler : SimpleTokenHandlerBase
  {
    // [@T[productline:TextTokenData]@T]
    private const string TOKEN_KEY = "productline";

    public override string TokenKey
    {
      get { return TOKEN_KEY; }
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      TokenEvaluationResult result = TokenEvaluationResult.Errors;
      const string errorSource = "ProductLine.EvaluateTokens";

      foreach (var token in tokens)
      {
        string tokenResult = null;
        var simpleToken = token as SimpleToken;

        if (simpleToken != null && !String.IsNullOrEmpty(simpleToken.RawTokenData))
        {
          try
          {
            IProductLineProvider productLineProvider = container.Resolve<IProductLineProvider>();

            var tokenDataFromRaw = simpleToken.RawTokenData.Split(':');

            int overrideFlag;
            if (tokenDataFromRaw.Length > 1 && int.TryParse(tokenDataFromRaw[1], out overrideFlag))
            {
              tokenResult = productLineProvider.GetProductLineName(tokenDataFromRaw[0], overrideFlag);
            }
            else
            {
              tokenResult = productLineProvider.GetProductLineName(tokenDataFromRaw[0]);
            }

            result = TokenEvaluationResult.Success;
          }
          catch (Exception)
          {
            result = TokenEvaluationResult.Errors;
            LogDebugMessage(container, "Could not deserialize token data.  Token: " + token.FullTokenString, errorSource);
          }

        }

        token.TokenResult = tokenResult ?? string.Empty;
      }

      return result;
    }

    private void LogDebugMessage(IProviderContainer container, string message, string errorSource)
    {
      IDebugContext debugContext = null;
      if (container.CanResolve<IDebugContext>())
      {
        debugContext = container.Resolve<IDebugContext>();
        debugContext.LogDebugTrackingData(errorSource, message);
      }
    }

  }
}
