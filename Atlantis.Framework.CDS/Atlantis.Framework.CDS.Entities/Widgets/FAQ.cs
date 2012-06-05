using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class FAQ : IWidgetModel
  {
    public FAQ()
    {
    }

    public string RightImage { get; set; }
    public string RightImageTitle { get; set; }
    public int RightImageWidth { get; set; }
    public int RightImageHeight { get; set; }
    public bool Filtered { get; set; }

    public List<FAQItem> FAQItems { get; set; }

    public class FAQItem : ElementBase
    {
      public FAQItem()
      {
        Title = string.Empty;
        Text = string.Empty;
      }

      public string Title { get; set; }
      public bool Filtered { get; set; }

      public List<FAQListItem> FAQListItems { get; set; }

      public class FAQListItem : ElementBase
      {
      }
    }
  }
}
