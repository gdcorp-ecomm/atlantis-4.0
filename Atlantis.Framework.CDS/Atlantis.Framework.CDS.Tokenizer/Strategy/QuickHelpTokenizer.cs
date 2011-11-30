using System.Collections.Generic;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Tokens;

namespace Atlantis.Framework.CDS.Tokenizer.Strategy
{
  public class QuickHelpTokenizer : ITokenizerStrategy
  {
    public string Process(List<string> tokens)
    {      
      return string.Format("<span data-qh=\\\"{0}\\\">{1}</span>", tokens[QuickHelpToken.QH_ID], tokens[QuickHelpToken.LINKTEXT]);
      //return "<span class=\"ndash\" onmouseover=\"atl_ShowQuickHelp(event, '" + tokens[QuickHelpToken.QH_ID] + "');\" onmouseout=\"atl_HideQuickHelp();\">" + tokens[QuickHelpToken.LINKTEXT] + "</span>";
    }
  }
}