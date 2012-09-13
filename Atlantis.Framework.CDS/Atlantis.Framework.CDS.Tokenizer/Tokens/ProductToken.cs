using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Strategy;
using System.Linq;

namespace Atlantis.Framework.CDS.Tokenizer.Tokens
{
  public class ProductToken : BaseToken, IUniversalToken
  {
    public const int TOKEN_TYPE = 0;
    public const int PRODUCT_ID = TOKEN_TYPE + 1;
    public const int OPERATOR = PRODUCT_ID + 1;
    public const int DROP_DECIMAL = OPERATOR + 1;
    public const int TERM_LABEL = DROP_DECIMAL + 1;

    // price was the original product pricing token which triggered current price,
    // but we needed to add the ability to support list price so "price_list"
    // for consistency sake, we added "price_current" as an option so it more clearly denotes intention; "price" == "price_current"
    public readonly List<string> VALID_OPERATORS = new List<string> { "price", "price_current", "price_list", "description" };
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

      if (tokens[OPERATOR].StartsWith("price"))
        _strategy = new ProductPriceTokenizer();
      else if (tokens[OPERATOR] == "description")
        _strategy = new ProductDescriptionTokenizer();

    }
  }
}
