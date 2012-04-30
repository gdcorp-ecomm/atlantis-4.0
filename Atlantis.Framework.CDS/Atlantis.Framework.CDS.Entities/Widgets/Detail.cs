using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class Detail : IWidgetModel
  {
    public List<DetailItem> Details { get; set; }

    public class DetailItem
    {
      public string Text { get; set; }
      public string Title { get; set; }
    }
  }
}
