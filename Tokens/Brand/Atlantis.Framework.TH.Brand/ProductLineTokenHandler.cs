using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Brand.Interface;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Brand
{
  // [@T[productline:<auctions contextid="GD"/>]@T]
  // [@T[productline:<auctions />]@T]
  public class ProductLineTokenHandler : XmlTokenHandlerBase
  {
    public override string TokenKey
    {
      get { return "productline"; }
    }

    public override IToken CreateToken(string tokenData, string fullTokenString)
    {
      return new ProductLineToken(TokenKey, tokenData, fullTokenString);
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      var result = TokenEvaluationResult.Errors;

      foreach (var token in tokens)
      {
        string tokenResult = null;
        var productLineToken = token as ProductLineToken;

        if (productLineToken != null && !String.IsNullOrEmpty(productLineToken.RawTokenData))
        {
          var brandProvider = container.Resolve<IBrandProvider>();
          int contextId = 0;

          if (!String.IsNullOrEmpty(productLineToken.ContextId))
          {
            switch (productLineToken.ContextId.ToUpperInvariant())
            {
              case "GD":
              case "1":
                contextId = 1;
                break;
              case "WWD":
              case "2":
                contextId = 2;
                break;
              case "BR":
              case "5":
                contextId = 5;
                break;
            }

            tokenResult = brandProvider.GetProductLineName(productLineToken.RenderType, contextId);
          }
          else
          {
            tokenResult = brandProvider.GetProductLineName(productLineToken.RenderType);
          }
        }

        token.TokenResult = tokenResult ?? String.Empty;
        result = TokenEvaluationResult.Success;
      }

      return result;
    }
  }
}
