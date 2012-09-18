using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using System.Text.RegularExpressions;
using Atlantis.Framework.CDS.Tokenizer.Strategy;

namespace Atlantis.Framework.CDS.Tokenizer.Tokens
{
  class ProductPackageToken : BaseToken, IUniversalToken
  {
    public const int TOKEN_TYPE = 0;
    public const int PRODUCT_IDS = TOKEN_TYPE + 1;
    public const int TERM_LABEL = PRODUCT_IDS + 1;
    public const int OPERATOR = TERM_LABEL + 1;
    public const int DROP_DECIMAL = OPERATOR + 1;

    public readonly List<string> VALID_TERM_OPERATORS = new List<string> { "monthly", "yearly" };
    public readonly List<string> VALID_OPERATORS = new List<string> { "price_current", "price_list" };
    public readonly List<string> VALID_DECIMAL_OPERATORS = new List<string> { "dropdecimal", "keepdecimal" };


    public ProductPackageToken(string originalToken) :
      base(originalToken)
    {
      Regex validateProductIds = new Regex(@"^[\d\|]+$"); // example: 759|807|3802

      if (!validateProductIds.IsMatch(tokens[PRODUCT_IDS]))
        throw new InvalidProgramException(String.Format("The product ID {0} was not a valid pipe delimited list of pfids.", tokens[PRODUCT_IDS]));

      if (!VALID_OPERATORS.Contains(tokens[OPERATOR]))
        throw new InvalidProgramException(String.Format("The operator {0} was not in the list of valid operators.", tokens[OPERATOR]));

      if (!VALID_TERM_OPERATORS.Contains(tokens[TERM_LABEL]))
        throw new InvalidProgramException(String.Format("The term label {0} was not in the list of valid term operators.", tokens[TERM_LABEL]));

      if (!VALID_DECIMAL_OPERATORS.Contains(tokens[DROP_DECIMAL]))
        throw new InvalidProgramException(String.Format("The term label {0} was not in the list of valid drop/keep decimal operators.", tokens[DROP_DECIMAL]));

      _strategy = new ProductPackageTokenizer();

    }
  }
}
