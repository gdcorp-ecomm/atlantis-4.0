using System.Collections.Generic;
using System.Text.RegularExpressions;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Tokens;

namespace Atlantis.Framework.CDS.Tokenizer
{
  public class CDSTokenizer
  {
    public CDSTokenizer(){}

    public string Parse(string input)
    {
      return Parse(input, null);
    }

    public string Parse(string input, IDictionary<string,string> customTokens)
    {
      if (customTokens != null)
      {
        foreach (var item in customTokens)
        {
          input = input.Replace(item.Key, item.Value);
        }
      }
      return ReplaceUniversalTokens(input);
    }

    private string ReplaceUniversalTokens(string input)
    {
      Regex getTokens = new Regex(@"\{\{(.*?)\}\}");

      MatchCollection tokens = getTokens.Matches(input);

      foreach (Match token in tokens)
      {
        IUniversalToken uToken = BaseToken.GetUniversalToken(token.ToString());

        if (uToken != null)
        {
          input = input.Replace(uToken.GetToken(), uToken.ToString());
        }
        else 
        {
          input = input.Replace(token.ToString(), string.Format("[ERROR, Unhandled Token: {0}]", token.ToString()));
        }
      }

      return input;
    }

  }
}
