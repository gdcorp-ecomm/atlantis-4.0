using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class QuickHelp : IWidgetModel
  {
    public QuickHelp()
    {
      QuickHelpItems = new List<QuickHelpItem>();
    }

    public List<QuickHelpItem> QuickHelpItems { get; set; }

    public class QuickHelpItem
    {
      public string QhId { get; set; }
      public string Text { get; set; }
    }
  }
}
