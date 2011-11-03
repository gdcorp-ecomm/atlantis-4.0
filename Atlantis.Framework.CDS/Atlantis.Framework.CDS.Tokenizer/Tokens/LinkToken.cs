using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Strategy;

namespace Atlantis.Framework.CDS.Tokenizer.Tokens
{
  public class LinkToken : BaseToken, IUniversalToken
  {
    public const int TOKEN_TYPE = 0;
    public const int LINK_TYPE = TOKEN_TYPE + 1;
    public const int URL = LINK_TYPE + 1;
    public const int SECURE = URL + 1;
    public const int QUERYSTRING = SECURE + 1;

    public readonly List<string> VALID_SECURE_VALUES = new List<string> { "true", "false" };

    public LinkToken(string originalToken) :
      base(originalToken)
    {
      Regex validateType = new Regex(@"^(\w+)$");

      if (!validateType.IsMatch(tokens[LINK_TYPE]))
        throw new InvalidProgramException(String.Format("The Link Type {0} was invalid.", tokens[LINK_TYPE]));

      if (!VALID_SECURE_VALUES.Contains(tokens[SECURE]))
        throw new InvalidProgramException(String.Format("The secure operator {0} is invalid.", tokens[SECURE]));

      _strategy = new LinkTokenizer();
    }
  }
}
