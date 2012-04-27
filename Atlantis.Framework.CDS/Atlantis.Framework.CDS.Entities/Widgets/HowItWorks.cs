using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;
using Atlantis.Framework.CDS.Entities.Attributes;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  [SectionContainer("How it Works")]
  public class HowItWorks : IWidgetModel
  {
    public HowItWorks()
    {
    }

    public string HowItWorksSpriteImage { get; set; }
    [HideInManagementUI]
    public bool Filtered { get; set; }

    public List<HowItWorksItem> HowItWorksItems { get; set; }

    public class HowItWorksItem : ElementBase
    {
      public HowItWorksItem()
      {
      }

      public string Title { get; set; }
      public string SpritePosition { get; set; }
      [HideInManagementUI]
      public bool Filtered { get; set; }

      public List<HowItWorksListItem> HowItWorksListItems { get; set; }

      public class HowItWorksListItem : ElementBase
      {
      }
    }
  }
}
