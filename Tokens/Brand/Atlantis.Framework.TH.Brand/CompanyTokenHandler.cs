using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Brand.Interface;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Brand
{
  // [@T[company:TextTokenData]@T]
  // [@T[company:TextTokenData:override]@T]
  public class CompanyTokenHandler : SimpleTokenHandlerBase
  {
    private const string TOKEN_KEY = "companyname";

    public override string TokenKey
    {
      get { return TOKEN_KEY; }
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      TokenEvaluationResult result = TokenEvaluationResult.Errors;
      const string errorSource = "Company.EvaluateTokens";

      foreach (var token in tokens)
      {
        string tokenResult = null;
        var simpleToken = token as SimpleToken;

        if (simpleToken != null && !String.IsNullOrEmpty(simpleToken.RawTokenData))
        {
          try
          {
            ICompanyProvider productLineProvider = container.Resolve<ICompanyProvider>();
            tokenResult = productLineProvider.GetCompanyPropertyValue(simpleToken.RawTokenData);
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
