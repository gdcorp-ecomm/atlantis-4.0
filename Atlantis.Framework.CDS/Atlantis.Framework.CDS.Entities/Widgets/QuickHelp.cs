using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class QuickHelp
  {
    public List<QuickHelpItem> QuickHelpItems { get; set; }

    public class QuickHelpItem
    {
      public string QhId { get; set; }
      public string Text { get; set; }
    }
  }
}
