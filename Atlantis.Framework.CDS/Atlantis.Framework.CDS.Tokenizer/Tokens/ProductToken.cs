using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Strategy;

namespace Atlantis.Framework.CDS.Tokenizer.Tokens
{
  public class ProductToken : BaseToken, IUniversalToken
  {
    public const int TOKEN_TYPE = 0;
    public const int PRODUCT_ID = TOKEN_TYPE + 1;
    public const int OPERATOR = PRODUCT_ID + 1;
    public const int DROP_DECIMAL = OPERATOR + 1;
    public const int TERM_LABEL = DROP_DECIMAL + 1;

    public readonly List<string> VALID_OPERATORS = new List<string> { "price", "description" };
    public readonly List<string> VALID_DECIMAL_OPERATORS = new List<string> { "dropdecimal", "keepdecimal" };
    public readonly List<string> VALID_TERM_OPERATORS = new List<string> { "monthly", "yearly" };

    protected enum TermParameter { Yearly, Monthly }

    public ProductToken(string originalToken) :
      base(originalToken)
    {
      Regex validateProductId = new Regex(@"^(\d+)$");

      if (!validateProductId.IsMatch(tokens[PRODUCT_ID]))
        throw new InvalidProgramException(String.Format("The product ID {0} was not numeric.", tokens[PRODUCT_ID]));

      if (!VALID_OPERATORS.Contains(tokens[OPERATOR]))
        throw new InvalidProgramException(String.Format("The operator {0} was not in the list of valid operators.", tokens[OPERATOR]));

      if (tokens[OPERATOR] == VALID_OPERATORS.Find(s => s == "price"))
        _strategy = new ProductPriceTokenizer();
      else if (tokens[OPERATOR] == VALID_OPERATORS.Find(s => s == "description"))
        _strategy = new ProductDescriptionTokenizer();

    }
  }
}
