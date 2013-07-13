using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.ShopperValidator.Interface.RuleConstants
{
  public static class DefaultFieldNameHelper
  {
    public static void OverwriteTextIfEmpty(string assignedText, string defaultText, out string newText)
    {
      newText = string.IsNullOrEmpty(assignedText) ? defaultText : assignedText;
    }
  }
}
