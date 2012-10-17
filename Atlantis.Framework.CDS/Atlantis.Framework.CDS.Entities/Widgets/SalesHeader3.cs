using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class SalesHeader3 : SalesHeader2, IWidgetModel, IContent
  {
    public SalesHeader3() { }

    public string Custom99PercentContent { get; set; }
    public int SubHeadTop { get; set; }

    public List<string> ContentList { get; set; }
  }
}
