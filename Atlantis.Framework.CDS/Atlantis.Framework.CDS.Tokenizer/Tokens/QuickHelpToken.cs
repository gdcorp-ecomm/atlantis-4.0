using System;
using System.Text.RegularExpressions;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Strategy;

namespace Atlantis.Framework.CDS.Tokenizer.Tokens
{
  public class QuickHelpToken : BaseToken, IUniversalToken
  {
    public const int TOKEN_TYPE = 0;
    public const int QH_ID = TOKEN_TYPE + 1;
    public const int LINKTEXT = QH_ID + 1;

    public QuickHelpToken(string originalToken) :
      base(originalToken)
    {
      Regex quickHelpItem = new Regex(@"^(\w+)$");

      if (!quickHelpItem.IsMatch(tokens[QH_ID]))
        throw new InvalidProgramException(String.Format("The quick help item {0} was not found.", tokens[QH_ID]));

      System.Diagnostics.Debug.WriteLine(tokens[QH_ID]);
      System.Diagnostics.Debug.WriteLine(tokens[LINKTEXT]);


      _strategy = new QuickHelpTokenizer();
    }
  }
}