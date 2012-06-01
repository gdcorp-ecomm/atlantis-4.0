using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class HowItWorks : IWidgetModel
  {
    public HowItWorks()
    {
    }

    public string HowItWorksSpriteImage { get; set; }
    public bool Filtered { get; set; }

    public List<HowItWorksItem> HowItWorksItems { get; set; }

    public class HowItWorksItem : ElementBase
    {
      public HowItWorksItem()
      {
      }

      public string Title { get; set; }
      public string SpritePosition { get; set; }
      public bool Filtered { get; set; }

      public List<HowItWorksListItem> HowItWorksListItems { get; set; }

      public class HowItWorksListItem : ElementBase
      {
      }
    }
  }
}
