using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Strategy;

namespace Atlantis.Framework.CDS.Tokenizer.Tokens
{
  public class PriceToken : BaseToken, IUniversalToken
  {
    public const int TOKEN_TYPE = 0;
    public const int CURRENCY_TYPE = TOKEN_TYPE + 1;
    public const int AMOUNT = CURRENCY_TYPE + 1;
    public const int DROP_DECIMAL = AMOUNT + 1;

    public readonly List<string> VALID_DECIMAL_OPERATORS = new List<string> { "dropdecimal", "keepdecimal" };

    public PriceToken(string originalToken) :
      base(originalToken)
    {
      Regex validatePrice = new Regex(@"^(\d+)$");

      if (!validatePrice.IsMatch(tokens[AMOUNT]))
        throw new InvalidProgramException(String.Format("The price {0} was not numeric.", tokens[AMOUNT]));

      if (!VALID_DECIMAL_OPERATORS.Contains(tokens[DROP_DECIMAL]))
        throw new InvalidProgramException(String.Format("The dropdecimal operator {0} is invalid.", tokens[DROP_DECIMAL]));

      _strategy = new PriceTokenizer();
    }
  }
}
