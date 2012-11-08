using Atlantis.Framework.Interface;
using Atlantis.Framework.Tokens.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.TH.Products
{
  class ProductCompareTokenHandler : XmlTokenHandlerBase
  {
    public override string TokenKey
    {
      get { return "productcompare"; }
    }
    
    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      TokenEvaluationResult result = TokenEvaluationResult.Success;
      return result;
    }
  }
}
