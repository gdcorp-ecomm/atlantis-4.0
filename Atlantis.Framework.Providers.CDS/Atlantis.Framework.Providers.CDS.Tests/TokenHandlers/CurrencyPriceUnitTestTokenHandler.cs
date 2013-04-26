using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.Providers.CDS.Tests.TokenHandlers
{
  public class CurrencyPriceUnitTestTokenHandler : XmlTokenHandlerBase
  {
    public override string TokenKey { get { return "currencyprice"; } }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      foreach (IToken token in tokens)
      {
        XElement currencyPriceElement = XElement.Load(new StringReader(token.RawTokenData));
        token.TokenResult = "$" + currencyPriceElement.Attribute("usdamount").Value;
      }

      return TokenEvaluationResult.Success;
    }
  }
}
