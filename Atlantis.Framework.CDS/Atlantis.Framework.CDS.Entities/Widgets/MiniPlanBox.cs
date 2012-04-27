using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Interfaces;

namespace Atlantis.Framework.CDS.Entities.Widgets
{
  public class MiniPlanBox : IWidgetModel
  {
    public string HeaderText { get; set; }
    public int TabIndex { get; set; }
    public List<Plan> Plans { get; set; }

    public class Plan
    {
      public string PlanName { get; set; }
      public string ProductPrice { get; set; }
      public string CustomDurationText { get; set; }
      public List<string> ListItems { get; set; }
      public bool IsYearly { get; set; }
      public int PlanTabIndex { get; set; }
      public string PlanBoxFormName { get; set; }
      public int? PlanBoxIndex { get; set; }
    }
  }
}
